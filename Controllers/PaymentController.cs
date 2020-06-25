using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using bkfc.Data;
using bkfc.Models;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;

namespace bkfc.Controllers
{
    public class PaymentController : Controller
    {
        private readonly bkfcContext _context;

        public PaymentController(bkfcContext context)
        {
            _context = context;
        }

        // GET: Payment
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var payments = from p in _context.Payment
                           select p;
            payments = payments.Where(payment => payment.UserId == userId);
            return View(await payments.ToListAsync());
        }

        // GET: Payment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payment/Create
        public IActionResult Create()
        {
            var cart = JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
            double money = 0;
            foreach (Item item in cart)
            {
                money += item.food.Price * item.quantity;
            }

            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMON3WL20200623";
            string accessKey = "0kA4tysnxO9tUKOx";
            string serectkey = "t2xCHeQr7pbnLBHqtutz5H1bt0hinODy";
            string orderInfo = "Đơn hàng từ Bách Khoa Food Court";
            string returnUrl = "https://localhost:5001/payment/done";
            string notifyurl = "https://localhost:5001/order";

            string amount = money.ToString();
            string orderid = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            //log.Debug("rawHash = "+ rawHash);

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);
            //log.Debug("Signature = " + signature);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };
            Console.WriteLine("Json request to MoMo: " + message.ToString());
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            Console.WriteLine(responseFromMomo);
            JObject jmessage = JObject.Parse(responseFromMomo);
            string payURL = responseFromMomo.Substring(207, 316);


            ViewData["cart"] = TempData["cart"] == null ? null : JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
            TempData.Keep();
            ViewData["payURL"] = payURL;
            return View();
        }
        private async Task<int> SaveOrderByVendor(List<Item> cart, Payment payment)
        {
            Dictionary<int, int> VendorOrderIndex = new Dictionary<int, int>();
            List<Order> Orders = new List<Order>();
            int i = 0;
            foreach (Item item in cart)
            {
                try
                {
                    int index = VendorOrderIndex[item.food.VendorId];
                }
                catch (KeyNotFoundException)
                {
                    VendorOrderIndex[item.food.VendorId] = i++;
                    Order order = new Order();
                    order.Status = "Cooking";
                    order.Date = DateTime.Now;
                    order.UserId = payment.UserId;
                    order.PaymentId = payment.Id;
                    order.VendorId = item.food.VendorId;
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    Orders.Add(order);
                }
            }
            foreach (Item item in cart)
            {
                int ind = VendorOrderIndex[item.food.VendorId];
                Order order = Orders[ind];
                Food food = await _context.Food.FindAsync(item.food.Id);
                if (food.OrderFoods == null) food.OrderFoods = new List<OrderFood>();
                food.OrderFoods.Add
                (
                    new OrderFood
                    {
                        Order = order,
                        Food = food,
                        Amount = item.quantity
                    }
                );
                await _context.SaveChangesAsync();
            }
            return 0;
        }
        // GET: Payement/Done
        // Momo will call this after payment succeded
        public async Task<ActionResult> Done(string amount, string errorCode)
        {
            if (errorCode == "0")
            {
                // food <=> payment
                var cart = JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
                Payment payment = new Payment();
                payment.Date = DateTime.Now;
                payment.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                payment.BalanceCharge = double.Parse(amount);
                _context.Add(payment);
                await _context.SaveChangesAsync();
                foreach (Item foodItem in cart)
                {
                    Food food = await _context.Food.FindAsync(foodItem.food.Id);
                    if (food.PaymentFoods == null) food.PaymentFoods = new List<PaymentFood>();
                    food.PaymentFoods.Add
                    (
                        new PaymentFood
                        {
                            Payment = payment,
                            Food = food,
                            Amount = foodItem.quantity
                        }
                    );
                    await _context.SaveChangesAsync();
                }

                // food <=> order
                await SaveOrderByVendor(cart, payment);
                // Xoa cart
                TempData["cart"] = null;
                TempData.Keep();
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Order");
            }
            else
            {
                return RedirectToAction("Create", "Payment");
            }

        }



        // POST: Payment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,BalanceCharge,UserId")] Payment payment)
        {
            //ViewData["cart"] = TempData["cart"] == null ? null : JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
            var cart = JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
            if (ModelState.IsValid)
            {
                payment.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                payment.Date = DateTime.Now;
                _context.Add(payment);
                await _context.SaveChangesAsync();
                // Tach Bill
                IList<int> vendors = new List<int>();
                foreach (Item item in cart)
                {
                    if (!vendors.Contains(item.food.VendorId))
                    {
                        vendors.Add(item.food.VendorId);
                    }
                }
                foreach (int vendorId in vendors)
                {
                    var order = new Order();
                    order.Status = "Cooking";
                    order.Date = DateTime.Now;
                    order.UserId = payment.UserId;
                    order.PaymentId = payment.Id;
                    order.VendorId = vendorId;

                    // foreach(Item item in cart) {
                    //     if (item.food.VendorId == order.VendorId) {
                    //         var orderFood = new OrderFood();
                    //         orderFood.Order = order;
                    //         orderFood.Food = item.food;
                    //         _context.Add(orderFood);
                    //         await _context.SaveChangesAsync();
                    //         order.OrderFoods.Add(orderFood);
                    //     }
                    // }
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                }
                // Xoa cart
                TempData["cart"] = null;
                TempData.Keep();
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Payment");
        }

        // GET: Payment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        // POST: Payment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,BalanceCharge,UserId,OrderId")] Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
        }

        // GET: Payment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payment.FindAsync(id);
            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.Id == id);
        }
    }
}
