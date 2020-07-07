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
using Microsoft.AspNetCore.Authorization;
using bkfc.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
namespace bkfc.Controllers
{
    [Authorize(Roles = "VendorManager, FoodCourtManager")]
    public class ReportController : Controller
    {
        private readonly bkfcContext _context;
        private readonly UserManager<bkfcUser> _userManager;
        public ReportController(bkfcContext context, UserManager<bkfcUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Report
        public IActionResult Index()
        {
            // ReportView myreport = new Repo
            ViewData["listVendor"] = _context.Vendor.ToList();
            return View(null);
        }
        // POST: Report
        [HttpPost]
        public async Task<IActionResult> Index(DateTime dateFrom, DateTime dateTo, int? vendorId)
        {
            var user =  await _userManager.GetUserAsync(User);
            DateTime MINDATE = new DateTime(2020, 06, 15);
            if (dateFrom < MINDATE) dateFrom = MINDATE; //protect database
            if (dateTo < dateFrom || dateTo > DateTime.Now.Date)
            {
                return NotFound();
            }
            int type = -1;
            if (vendorId != null)
            {
                type = (int)vendorId;
            }
            if(User.IsInRole("VendorManager"))
            {
                type = user.vendorid;
            }
            List<ReportEachVendor> vendorsReport = new List<ReportEachVendor>();
            if (type == -1) //whole food court 
            {
                foreach (var vendor in _context.Vendor.ToList())
                {
                    vendorsReport.Add(new ReportEachVendor
                    {
                        vendorId = vendor.Id,
                        vendorName = vendor.Name
                    });
                }
            }
            else
            {
                var vendor = _context.Vendor.Find(type);
                vendorsReport.Add(new ReportEachVendor
                {
                    vendorId = type,
                    vendorName = vendor.Name
                });
            }

            var reports = from m in _context.Report
                          select m;
            var myReport = new ReportView();
            foreach (var vendorReport in vendorsReport)
            {
                List<Food> listFood = new List<Food>();
                double income = 0;
                var vendorID = vendorReport.vendorId;
                for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
                {
                    var re = await reports.Where(r => r.Date == date && r.VendorId == vendorID).ToListAsync();
                    Report report = new Report();
                    if (re.Count == 0 || date == DateTime.Now.Date)
                    {
                        report = createReport(date, vendorID);
                    }
                    else
                    {
                        report = re[0];
                    }
                    var listOrder = JsonConvert.DeserializeObject<IList<Order>>(report.FoodSold);
                    foreach (Order order in listOrder)
                    {
                        var particularOrder = from m in _context.OrderFoods
                                              select m;
                        particularOrder = particularOrder.Where(particularOrder => particularOrder.OrderId == order.Id);
                        List<OrderFood> particularOrderList = particularOrder.ToList();
                        foreach (OrderFood pOrder in particularOrderList)
                        {
                            Food foodToAdd = await _context.Food.FindAsync(pOrder.FoodId);
                            foodToAdd.Amount = pOrder.Amount;
                            var existedFood = listFood.Find(x => x.Id == foodToAdd.Id);
                            if (existedFood != null) { existedFood.Amount += pOrder.Amount; }
                            else { listFood.Add(foodToAdd); }


                            income += foodToAdd.Price * pOrder.Amount;
                        }
                    }
                }
                vendorReport.listFood = listFood;
                vendorReport.Income = income;
            }
            ViewData["listVendor"] = _context.Vendor.ToList();
            myReport.reports = vendorsReport;
            myReport.dayStart = dateFrom;
            myReport.dayEnd = dateTo;
            return View(myReport);
        }

        private Report createReport(DateTime date, int vendorId)
        {
            var orders = from m in _context.Order
                         select m;
            if (vendorId == -1) //whole food court
            {
                orders = orders.Where(orders => orders.Date.Date == date);
            }
            else orders = orders.Where(orders => orders.Date.Date == date && orders.VendorId == vendorId);
            // await orders.ToListAsync();
            List<Order> orderList = orders.ToList();
            double Income = 0;
            var reports = from m in _context.Report
                          select m;
            Report report = new Report();
            var re = reports.Where(r => r.Date == date && r.VendorId == vendorId).ToList();
            if (re.Count > 0) //if found any report
            {
                report = re[0];
                report.FoodSold = JsonConvert.SerializeObject(orderList);
                report.Income = Income;
                _context.Update(report);
                _context.SaveChanges();
                return report;
            }
            report.Date = date.Date;
            report.Name = date.ToString();
            report.FoodSold = JsonConvert.SerializeObject(orderList);
            report.VendorId = vendorId;
            report.Income = Income; //nhảm nhí
            _context.Add(report);
            _context.SaveChanges();
            return report;
        }
        // GET: Report/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Report/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Report/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Date,FoodSold,Income,VendorId")] Report report)
        {
            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // GET: Report/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        // POST: Report/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Date,FoodSold,Income,VendorId")] Report report)
        {
            if (id != report.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id))
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
            return View(report);
        }

        // GET: Report/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Report/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Report.FindAsync(id);
            _context.Report.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Report.Any(e => e.Id == id);
        }
    }
}
