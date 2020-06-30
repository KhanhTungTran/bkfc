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

namespace bkfc.Controllers
{
    public class OrderController : Controller
    {
        private readonly bkfcContext _context;

        public OrderController(bkfcContext context)
        {
            _context = context;
        }

        // GET: Order
        ///**** fix cho nayy
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var orders = from m in _context.Order
                         select m;
            orders = orders.Where(orders => orders.UserId == userId);
            await orders.ToListAsync();
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

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,Date,UserId,PaymentId,VendorId, OrderFoods")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.Status = "Cooking";
                order.Date = DateTime.Now;
                order.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        // GET: Oder/UpdateOrderStatus
        public async Task<IActionResult> UpdateOrderStatus(int? vendorId)
        {
            
            var orders = from m in _context.Order
                         select m;
            if (vendorId!=null)
            {
                orders = orders.Where(orders => orders.VendorId == vendorId);
            }
            await orders.ToListAsync();
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
        public async Task<IActionResult> UpdateOrderStatus(int? orderId, string status)
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
            return RedirectToAction("UpdateOrderStatus",new{vendorId = order.VendorId});

        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,Date,UserId,PaymentId,VendorId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
