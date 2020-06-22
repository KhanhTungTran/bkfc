using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bkfc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bkfc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult Create()
        {
            //Response.Redirect("/");
            return RedirectToPage("/foodcourt");
            return View("~/Areas/Identity/Pages/Account/Register.cshtml");
        }
    }
}
