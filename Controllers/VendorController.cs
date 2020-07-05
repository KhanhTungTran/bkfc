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
using bkfc.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace bkfc.Controllers
{
    public class VendorController : Controller
    {
        private readonly bkfcContext _context;
        private readonly UserManager<bkfcUser> _userManager;

        public VendorController(bkfcContext context, UserManager<bkfcUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Index
        public async Task<IActionResult> Index(int? id)
        {
            // check permission here
            var vendor = await _context.Vendor.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            var foods = from f in _context.Food
                        select f;
            foods = foods.Where(f => f.VendorId == id);
            ViewData["food"] = foods.ToList();
            return View(vendor);
        }
        // GET: Foodcourt/Edit/5
        [Authorize(Roles = "FoodCourtManager,Admin,VendorManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendor.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }
            return View(vendor);
        }

        // POST: Foodcourt/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FoodCourtManager,Admin,VendorManager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Logo,Category")] Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendorExists(vendor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = id });
            }
            return View(vendor);
        }



        // GET: Vendor/DetailsFood/5
        public async Task<IActionResult> DetailsFood(int? id)
        {
            ViewData["vendorId"] = id;
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
        public IActionResult CreateFood(int? id)
        {
            ViewData["vendorid"] = id;
            return View();
        }

        // POST: Food/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFood([Bind("VendorId,Name,Image,Description,Price,Amount,Discount")] Food food)
        {
            if (ModelState.IsValid)
            {
                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = food.VendorId });
            }
            return View(food);
        }

        // GET: Food/Edit/5
        public async Task<IActionResult> EditFood(int? id)
        {
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
        public async Task<IActionResult> EditFood(int id, [Bind("Id,VendorId,Name,Image,Description,Price,Amount,Discount")] Food food)
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
                        Console.WriteLine("ASDasdasdasda");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = food.VendorId });
            }
            return RedirectToAction("Index", new { id = food.VendorId });
        }

        // GET: Food/Delete/5
        public async Task<IActionResult> DeleteFood(int? id, int? vendorId)
        {
            ViewData["vendorId"] = vendorId;
            ViewData["vendor"] = _context.Vendor.Find(vendorId);
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
        [HttpPost, ActionName("DeleteFood")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var food = await _context.Food.FindAsync(id);
            _context.Food.Remove(food);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = food.VendorId });
        }

        // GET: Vendor/StaffManagement/:id
        public async Task<IActionResult> StaffManagement(int? id)
        {
            // check permission here
            var vendor = await _context.Vendor.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }
            ViewData["result"] = " ";
            return View(vendor);
        }
        [HttpPost]
        public async Task<IActionResult> StaffManagementAsync(int vendorId, string mail)
        {
            var vendor = await _context.Vendor.FindAsync(vendorId);
            if (vendor == null)
            {
                return NotFound();
            }
            if (mail == null)
            {
                ViewData["result"] = "mail is empty";
                return View(vendor);
            }
            Task<int> task = AddStaff(mail, vendorId);
            task.Wait();
            var res = task.Result;
            ViewData["result"] = "Add successfully";
            return View(vendor);
        }
        public async Task<int> AddStaff(string mail, int vendorId)
        {
            bkfcUser user = await _userManager.FindByEmailAsync(mail);
            if (user == null) return 2;
            await _userManager.AddToRoleAsync(user, "Staff");
            return 0;
        }
        private bool FoodExists(int id)
        {
            return _context.Food.Any(e => e.Id == id);
        }

        private bool VendorExists(int id)
        {
            return _context.Vendor.Any(e => e.Id == id);
        }
    }
}
