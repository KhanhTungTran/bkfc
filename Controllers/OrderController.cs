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
using bkfc.Areas.Identity.Data;
namespace bkfc.Controllers
{
    public class OrderController : Controller
    {
        const int MIN_PER_FOOD = 2;
        private readonly bkfcContext _context;
        private readonly UserManager<bkfcUser> _userManager;
        public OrderController(bkfcContext context, UserManager<bkfcUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Order
        public async Task<IActionResult> Index(string typeDate="ToDay")
        {
            ViewData["typeDate"] = typeDate;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var orders = from m in _context.Order
                         select m;
            switch(typeDate)
            {
                case "ToDay":
                    orders = orders.Where(orders => orders.Date.Date==DateTime.Now.Date);
                    break;
                case "7Day":
                    orders = orders.Where(orders =>orders.Date.Date>DateTime.Now.Date.AddDays(-7));
                    break;
                case "All":
                    break;
            }
            await orders.ToListAsync();
            List<Order> orderList = orders.ToList();
            List<MyOrderFood> myOrderFoods = new List<MyOrderFood>();
            Dictionary<int,int> estimate = new Dictionary<int, int>();
            foreach (Order order in orderList)
            {
                if(!estimate.ContainsKey(order.VendorId)){
                    estimate.Add(order.VendorId,0);
                }
                var particularOder = from m in _context.OrderFoods
                                     select m;
                particularOder = particularOder.Where(particularOder => particularOder.OrderId == order.Id);
                List<OrderFood> particularOderList = particularOder.ToList();
                List<Food> foodList = new List<Food>();
                foreach (OrderFood pOrder in particularOderList)
                {
                    Food foodToAdd = await _context.Food.FindAsync(pOrder.FoodId);
                    foodToAdd.Amount = pOrder.Amount;
                    if (order.Status == "Cooking" && order.Date.Date == DateTime.Now.Date)
                    {
                        estimate[foodToAdd.VendorId]+=foodToAdd.Amount*MIN_PER_FOOD;
                    }
                    foodList.Add(foodToAdd);
                }
                if(order.UserId == userId){
                    myOrderFoods.Add
                    (
                        new MyOrderFood
                        {
                            Id = order.Id,
                            Status = order.Status,
                            Foods = foodList,
                            Date = order.Date,
                            EstimateMinutes = estimate[order.VendorId]
                        }
                    );
                }
            }
            return View(myOrderFoods);
        }
        // GET: Oder/UpdateOrderStatus
        [Authorize(Roles = "VendorManager,Staff")]
        public async Task<IActionResult> UpdateOrderStatus(string typeDate="ToDay")
        {
            ViewData["typeDate"] = typeDate;
            var user =  await _userManager.GetUserAsync(User);
            int vendorId = user.vendorid;
            var orders = from m in _context.Order
                         select m;
            switch(typeDate)
            {
                case "ToDay":
                    orders = orders.Where(orders => orders.VendorId==vendorId && orders.Date.Date==DateTime.Now.Date);
                    break;
                case "7Day":
                    orders = orders.Where(orders => orders.VendorId==vendorId && orders.Date.Date>DateTime.Now.Date.AddDays(-7));
                    break;
                case "All":
                    orders = orders.Where(orders => orders.VendorId==vendorId);
                    break;
            }
            List<Order> orderList = orders.ToList();
            List<MyOrderFood> myOrderFoods = new List<MyOrderFood>();
            foreach (Order order in orderList)
            {
                var particularOder = from m in _context.OrderFoods
                                     select m;
                particularOder = particularOder.Where(particularOder => particularOder.OrderId == order.Id);
                List<OrderFood> particularOderList = particularOder.ToList();
                List<Food> foodList = new List<Food>();
                foreach (OrderFood pOrder in particularOderList)
                {
                    Food foodToAdd = await _context.Food.FindAsync(pOrder.FoodId);
                    foodToAdd.Amount = pOrder.Amount;
                    foodList.Add(foodToAdd);
                }
                myOrderFoods.Add
                (
                    new MyOrderFood
                    {
                        Id = order.Id,
                        Status = order.Status,
                        Foods = foodList,
                        Date = order.Date
                    }
                );
            }
            return View(myOrderFoods);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //POST: Order/UpdateOrderStatus?orderId=orderId&status=status
        public async Task<IActionResult> UpdateOrderStatus(int? orderId, string status, string typeDate)
        {
            if (orderId == null || status == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = status;
            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("UpdateOrderStatus",new{typeDate = typeDate});

        }
        [HttpPost]
        public async Task<string> setToken(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            user.Token = id;
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
            return id;
        }
    }
}