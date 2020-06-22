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
            return View();
        }
        public int increase(int foodId)
        {
            for (int i =0 ; i<Cart.cart.Count ; i++)
            {
                if (foodId == Cart.cart[i].food.Id)
                {
                    Cart.cart[i].quantity++;
                    ViewData["cart"]=Cart.cart;
                    return Cart.cart[i].quantity;
                }
            }
            return -1;
        }
        public int decrease(int foodId)
        {
            for (int i =0 ; i<Cart.cart.Count ; i++)
            {
                if (foodId == Cart.cart[i].food.Id)
                {
                    if(Cart.cart[i].quantity == 1)
                    {
                        remove(foodId);
                        return 0;
                    }
                    else
                    {
                        Cart.cart[i].quantity-- ;
                        ViewData["cart"]=Cart.cart;
                        return Cart.cart[i].quantity;
                    }
                    
                }
            }
            return -1;
        }
        public void change(int foodId, int quantity)
        {
            for (int i =0 ; i<Cart.cart.Count ; i++)
            {
                if (foodId == Cart.cart[i].food.Id)
                {
                    Cart.cart[i].quantity = quantity;
                    return;
                }
            }
        }
        public void remove(int foodId)
        {
            for (int i =0 ; i<Cart.cart.Count ; i++)
            {
                if (foodId == Cart.cart[i].food.Id)
                {
                    Cart.cart.Remove(Cart.cart[i]);
                    return;
                }
            }
        }
    }
}