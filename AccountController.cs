using Microsoft.AspNetCore.Mvc;
using EasyShop2.Data;
using EasyShop2.Models;
using System.Linq;

namespace EasyShop2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ShopContext _db;

        public AccountController(ShopContext db)
        {
            _db = db;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (_db.Users.Any(u => u.Username == user.Username))
            {
                ViewBag.Error = "Потребителското име вече съществува!";
                return View();
            }

            user.Role = "User"; // Всички нови потребители са стандартни
            _db.Users.Add(user);
            _db.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                return RedirectToAction("Index", "Products");
            }

            ViewBag.Error = "Грешно потребителско име или парола!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
