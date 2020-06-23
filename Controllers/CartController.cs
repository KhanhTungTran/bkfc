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
            ViewData["cart"] = TempData["cart"] == null ? null : JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
            TempData.Keep();
            return View();
        }
        public int increase(int foodId)
        {
            var cart = TempData["cart"] == null ? null : JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
            int returnVal = -1;
            for (int i =0 ; i<cart.Count ; i++)
            {
                if (foodId == cart[i].food.Id)
                {
                    cart[i].quantity++;
                    returnVal = cart[i].quantity;
                    break;
                }
            }
            TempData["cart"] = JsonConvert.SerializeObject(cart);
            ViewData["cart"] = cart;
            TempData.Keep();
            return returnVal;
        }
        public int decrease(int foodId)
        {
            var cart = TempData["cart"] == null ? null : JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
            int returnVal = -1;
            for (int i =0 ; i<cart.Count ; i++)
            {
                if (foodId == cart[i].food.Id)
                {
                    if(cart[i].quantity == 1)
                    {
                        remove(foodId);
                        returnVal = 0;
                        break;
                    }
                    else
                    {
                        cart[i].quantity-- ;
                        ViewData["cart"]=cart;
                        returnVal = cart[i].quantity;
                        break;
                    }       
                }
            }
            TempData["cart"] = JsonConvert.SerializeObject(cart);
            ViewData["cart"] = cart;
            TempData.Keep();
            return returnVal;
        }
        public void change(int foodId, int quantity)
        {
            var cart = TempData["cart"] == null ? null : JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
            for (int i =0 ; i<cart.Count ; i++)
            {
                if (foodId == cart[i].food.Id)
                {
                    cart[i].quantity = quantity;
                    break;
                }
            }
            TempData["cart"] = JsonConvert.SerializeObject(cart);
            ViewData["cart"] = cart;
            TempData.Keep();
        }
        public void remove(int foodId)
        {
            var cart = TempData["cart"] == null ? null : JsonConvert.DeserializeObject<List<Item>>(TempData["cart"] as string);
            for (int i =0 ; i<cart.Count ; i++)
            {
                if (foodId == cart[i].food.Id)
                {
                    cart.Remove(cart[i]);
                    break;
                }
            }
            TempData["cart"] = JsonConvert.SerializeObject(cart);
            ViewData["cart"] = cart;
            TempData.Keep();
        }
    }
}