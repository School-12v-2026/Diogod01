using Microsoft.AspNetCore.Mvc;
using EasyShop2.Data;
using EasyShop2.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;

namespace EasyShop2.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopContext _db;

        public CartController(ShopContext db)
        {
            _db = db;
        }

        // ------------------------
        // Показване на количката
        // ------------------------
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetString("Cart");
            List<Product> productsInCart = new List<Product>();

            if (!string.IsNullOrEmpty(cart))
            {
                var ids = cart.Split(',').Select(int.Parse).ToList();
                productsInCart = _db.Products.Where(p => ids.Contains(p.Id)).ToList();
            }

            return View(productsInCart);
        }

        // ------------------------
        // Премахване на продукт от количката
        // ------------------------
        [HttpPost]
        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(cart))
            {
                var ids = cart.Split(',').ToList();
                ids.Remove(id.ToString());
                HttpContext.Session.SetString("Cart", string.Join(",", ids));
            }

            return RedirectToAction("Index");
        }

        // ------------------------
        // Финализиране на поръчката
        // ------------------------
        [HttpPost]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cart))
            {
                TempData["Message"] = "Количката е празна!";
                return RedirectToAction("Index");
            }

            // Тук можеш да добавиш логика за запис на поръчката в база
            HttpContext.Session.Remove("Cart");
            TempData["Message"] = "Поръчката е финализирана успешно!";
            return RedirectToAction("Index");
        }
    }
}
