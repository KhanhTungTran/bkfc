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
        [Authorize(Roles = "FoodCourtManager,VendorManager,Staff")]
        public async Task<IActionResult> Index(int? id)
        {
            // check permission here
            if(!authenticateVendorId(id).Result)
            {
                return NotFound();
            }
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
        [Authorize(Roles = "FoodCourtManager,VendorManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if(!authenticateVendorId(id).Result)
            {
                return NotFound();
            }
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
        [Authorize(Roles = "FoodCourtManager,VendorManager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Logo,Category")] Vendor vendor)
        {
            if(!authenticateVendorId(id).Result)
            {
                return NotFound();
            }
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
            if(!authenticateVendorId(food.VendorId).Result)
            {
                return NotFound();
            }
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }


        // GET: Food/Create
        [Authorize(Roles = "FoodCourtManager,VendorManager")]
        public IActionResult CreateFood(int? id)
        {
            if(!authenticateVendorId(id).Result)
            {
                return NotFound();
            }
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
                if(!authenticateVendorId(food.VendorId).Result)
                {
                    return NotFound();
                }
                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = food.VendorId });
            }
            return View(food);
        }

        // GET: Food/Edit/5
        [Authorize(Roles = "FoodCourtManager,VendorManager")]
        public async Task<IActionResult> EditFood(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food.FindAsync(id);
            if(!authenticateVendorId(food.VendorId).Result)
            {
                return NotFound();
            }
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
            if(!authenticateVendorId(food.VendorId).Result)
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
        [Authorize(Roles = "FoodCourtManager,VendorManager")]
        public async Task<IActionResult> DeleteFood(int? id, int? vendorId)
        {
            if(!authenticateVendorId(vendorId).Result)
            {
                return NotFound();
            }
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
            if(!authenticateVendorId(food.VendorId).Result)
            {
                return NotFound();
            }
            _context.Food.Remove(food);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = food.VendorId });
        }

        // GET: Vendor/StaffManagement/:id
        [Authorize(Roles = "FoodCourtManager,VendorManager")]
        public async Task<IActionResult> StaffManagement(int? id)
        {
            // check permission here
            if(!authenticateVendorId(id).Result)
            {
                return NotFound();
            }
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
            if(!authenticateVendorId(vendorId).Result)
            {
                return NotFound();
            }
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
            if (res == 0) ViewData["result"] = "Add successfully";
            if (res == 1) ViewData["result"] = $"Fail - can't find email: {mail}";
            if (res == 2) ViewData["result"] = $"Fail - can't find vendorId: {vendorId}";
            return View(vendor);
        }
        public async Task<int> AddStaff(string mail, int vendorId)
        {
            bkfcUser user = await _userManager.FindByEmailAsync(mail);
            if (user == null) return 1;
            await _userManager.AddToRoleAsync(user, "Staff");
            Vendor vendor = await _context.Vendor.FindAsync(vendorId);
            if (vendor == null) return 2;
            user.vendorid = vendorId;
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
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
        private async Task<bool> authenticateVendorId(int? id)
        {
            var user =  await _userManager.GetUserAsync(User);
            if (id == null)
            {
                return false;
            }
            else{
                if (user.vendorid != id && !User.IsInRole("FoodCourtManager"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
