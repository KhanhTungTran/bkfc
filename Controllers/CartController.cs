using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bkfc.Data;
using bkfc.Models;

namespace bkfc.Controllers
{
    public class CartController : Controller
    {
        private readonly bkfcContext _context;

        public CartController(bkfcContext context)
        {
            _context = context;
        }

        // GET: Food
        public ViewResult Index()
        {
            ViewData["cart"] = Cart.cart;
            return View(Cart.cart);
        }
    }
}