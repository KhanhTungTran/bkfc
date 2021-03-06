using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bkfc.Data;
using bkfc.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace bkfc.Controllers
{

    public class FoodcourtController : Controller
    {
        private readonly bkfcContext _context;

        public FoodcourtController(bkfcContext context)
        {
            _context = context;
        }

        // GET: Foodcourt
        [AllowAnonymous]
        public async Task<IActionResult> Index(string vendorCategory="", string searchString="")
        {
            // Use LINQ to get list of Category
            ViewData["cart"] = TempData["cart"] == null ? null : JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
            TempData.Keep("cart");
            IQueryable<string> categoryQuery = from m in _context.Vendor
                                               orderby m.Category
                                               select m.Category;

            var vendors = from m in _context.Vendor
                          select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                vendors = vendors.Where(v => v.Name.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(vendorCategory))
            {
                vendors = vendors.Where(v => (v.Category.Contains(vendorCategory)));
            }

            var vendorCategoryVM = new VendorCategoryViewModel
            {
                Category = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                Vendors = await vendors.ToListAsync()
            };
            return View(vendorCategoryVM);
            // method ToListAsync() is when and where query is executed
        }

        // GET: Foodcourt/Details/5
        //[HttpPost]
        public async Task<IActionResult> Details(int? id, int? priceFrom, int? priceTo,int? discountFrom, string searchString = "")   // int?: nullable type
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendor == null)
            {
                return NotFound();
            }
            var foods = from f in _context.Food
                        select f;
            foods = foods.Where(f => f.VendorId == vendor.Id);
            if (!String.IsNullOrEmpty(searchString))
            {
                foods = foods.Where(v => v.Name.Contains(searchString));
            }
            if (priceFrom != null)
            {
                foods = foods.Where(f => f.Price >= priceFrom);
            }
            if (priceTo != null)
            {
                foods = foods.Where(f => f.Price <= priceTo);
            }
            if (discountFrom != null)
            {
                foods = foods.Where(f => f.Discount >= discountFrom);
            }
            ViewData["vendor"] = vendor;
            TempData.Keep("msg");
            return View(await foods.ToListAsync());
        }

        // GET: Foodcourt/Create
        [Authorize(Roles = "FoodCourtManager,Admin,VendorManager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Foodcourt/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FoodCourtManager,Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Logo,Category")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vendor);
        }
        // GET: Foodcourt/Delete/5
        [Authorize(Roles = "FoodCourtManager,Admin,VendorManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }
        // POST: Foodcourt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FoodCourtManager,Admin,VendorManager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendor = await _context.Vendor.FindAsync(id);
            _context.Vendor.Remove(vendor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendorExists(int id)
        {
            return _context.Vendor.Any(e => e.Id == id);
        }

        public void AddToCart(int foodId, int quantity)
        {
            var food = _context.Food.Find(foodId);

            var cart = TempData["cart"] == null ? null : JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
            if (cart == null)
            {
                cart = new List<Item>();
            }
            // var cart = TempData["cart"] as List<Item>;
            Item result = null;
            result = cart.Find(i => i.food.Id == food.Id);
            if (result == null)
            {
                cart.Add(new Item()
                {
                    food = food,
                    quantity = quantity,
                });
            }

            else{
                result.quantity += quantity;

            }
            ViewData["cart"] = cart;
            TempData["cart"] = JsonConvert.SerializeObject(cart);
            TempData.Keep("cart");
        }
    }
}
