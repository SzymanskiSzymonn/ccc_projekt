using ccc_projekt.Data;
using ccc_projekt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ccc_projekt.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountsController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public IActionResult Register()
        {
            // Przykładowa lista ról do dropdowna
            var roles = new List<SelectListItem>
    {
        new SelectListItem { Value = "User", Text = "User" },
        new SelectListItem { Value = "Project Manager", Text = "Project Manager" }
    };

            ViewBag.Roles = roles;

            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {

          

            if (_context.User.Any(u => u.Username == user.Username))
            {
                ModelState.AddModelError("Username", "Username already exists");
            }

            if (_context.User.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
            }

            if (!ModelState.IsValid)
            {
                var roles = new List<SelectListItem>
    {
        new SelectListItem { Value = "User", Text = "User" },
        new SelectListItem { Value = "Project Manager", Text = "Project Manager" }
    };

                ViewBag.Roles = roles;
                return View(user);
            }

            // Hashowanie hasła
            user.Password = _passwordHasher.HashPassword(user, user.Password);

            _context.User.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.User.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                if (verificationResult == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);
                    HttpContext.Session.SetString("UserEmail", user.Email);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

