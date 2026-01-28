using Microsoft.AspNetCore.Mvc;
using EasyShop2.Data;
using EasyShop2.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace EasyShop2.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShopContext _db;

        public ProductsController(ShopContext db)
        {
            _db = db;
        }

        // ------------------------
        // Index + Търсачка
        // ------------------------
        public IActionResult Index(string search)
        {
            var products = _db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search));
            }

            return View(products.ToList());
        }

        // ------------------------
        // Create продукт (Admin)
        // ------------------------
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile ImageFile)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Index");

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                product.ImageUrl = "/images/" + fileName;
            }

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // ------------------------
        // Edit продукт (Admin)
        // ------------------------
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Index");

            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile ImageFile)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Index");

            var dbProduct = _db.Products.FirstOrDefault(p => p.Id == product.Id);
            if (dbProduct == null)
                return NotFound();

            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                dbProduct.ImageUrl = "/images/" + fileName;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // ------------------------
        // Delete продукт (Admin)
        // ------------------------
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Index");

            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // ------------------------
        // Add to Cart (Всички потребители)
        // ------------------------
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            // Сесийна количка като CSV низ
            var cart = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cart))
                cart = id.ToString();
            else
                cart += "," + id;

            HttpContext.Session.SetString("Cart", cart);

            TempData["Message"] = "Продуктът беше добавен в количката!";
            return RedirectToAction("Index");
        }
    }
}
