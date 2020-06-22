using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bkfc.Data;
using bkfc.Models;
using Newtonsoft.Json;

namespace bkfc.Controllers
{
    public class FoodController : Controller
    {
        private readonly bkfcContext _context;

        public FoodController(bkfcContext context)
        {
            _context = context;
        }
        
        // GET: Food
        public async Task<IActionResult> Index(int? vendorId)
        {
            ViewData["vendorId"] = vendorId;
            if (vendorId != null)
            {
                var foods = from f in  _context.Food
                            select f;
                foods = foods.Where(f => f.VendorId == vendorId) ;
                return View(await foods.ToListAsync());
            }
            return View(await _context.Food.ToListAsync());
        }

        // GET: Food/Details/5
        public async Task<IActionResult> Details(int? id, int? vendorId)
        {
            ViewData["vendorId"] = vendorId;
            ViewData["cart"] = TempData["cart"] == null ? null : JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
            TempData.Keep();
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // GET: Food/Create
        public IActionResult Create(int? id)
        {
            ViewData["vendorId"] = id;
            return View();
        }

        // POST: Food/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendorId,Name,Image,Description,Price,Amount,Discount")] Food food)
        {
            if (ModelState.IsValid)
            {
                _context.Add(food);
                await _context.SaveChangesAsync();      
                return RedirectToAction("Index", new{vendorid = food.VendorId});
            }
            return View(food);
        }

        // GET: Food/Edit/5
        public async Task<IActionResult> Edit(int? id, int? vendorId)
        {
            ViewData["vendorId"] = vendorId;
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        // POST: Food/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VendorId,Name,Image,Description,Price,Amount,Discount")] Food food)
        {
            if (id != food.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new{vendorid = food.VendorId});
            }
            return View(food);
        }

        // GET: Food/Delete/5
        public async Task<IActionResult> Delete(int? id, int? vendorId)
        {
            ViewData["vendorId"] = vendorId;
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }
            ViewData["vendorId"] = food.VendorId;
            return View(food);
        }

        // POST: Food/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var food = await _context.Food.FindAsync(id);
            _context.Food.Remove(food);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new{vendorid = food.VendorId});
        }

        private bool FoodExists(int id)
        {
            return _context.Food.Any(e => e.Id == id);
        }
    }
}
