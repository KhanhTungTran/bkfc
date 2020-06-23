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
            return View(await _context.Payment.ToListAsync());
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
            ViewData["cart"] = TempData["cart"] == null ? null : JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
            TempData.Keep();
            return View();
        }

        // POST: Payment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,BalanceCharge,UserId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                payment.Date = DateTime.Now;
                _context.Add(payment);

                var cart = JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
                // Tach Bill
                IList<int> vendors = new List<int>();
                foreach(Item item in cart) {
                    if (!vendors.Contains(item.food.VendorId)) {
                        vendors.Add(item.food.VendorId);
                    }
                }

                foreach(int vendorId in vendors) {
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




                TempData["cart"] = null;
                TempData.Keep();
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
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
