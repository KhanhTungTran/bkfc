using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bkfc.Areas.Identity.Data;
using bkfc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace bkfc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly SignInManager<bkfcUser> _signInManager;
        private readonly UserManager<bkfcUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(UserManager<bkfcUser> userManager,SignInManager<bkfcUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Index(string mail, string role)
        {
            Task <int> task = AssignRoleToUserr(mail, role);
            task.Wait();
            var res = task.Result;
            return Content($"{res}  ");
        }
        private async Task<int> AssignRoleToUserr(string mail, string role)
        {
            //var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roleCheck = await _roleManager.RoleExistsAsync(role);
            if (!roleCheck) return 1;
            bkfcUser user = await _userManager.FindByEmailAsync(mail);
            if (user == null) return 2;
            await _userManager.AddToRoleAsync(user, role);
            return 0;
        }
    }
}
