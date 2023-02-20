using HospitalSanJose.Models;
using HospitalSanJose.Models.Auth;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using NuGet.Common;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HospitalSanJose.Controllers
{
    public class AuthController : Controller
    {

        private readonly HospitalDbContext _context;

        public AuthController(HospitalDbContext context)
        {

            _context = context;
        }

        public IActionResult Login()
        {
            ViewBag.ShowWarning = false;
            return View("Login/Index");
        }

        public IActionResult Register()
        {
            return View("Register/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login formData)
        {
            ViewBag.ShowWarning = false;
            if (!ModelState.IsValid)
                // If the model state is invalid, redisplay the form
                return View("Login/Index");

            // Look up the user in the database
            var user = _context.Users.FirstOrDefault(u => u.Username == formData.Username);

            if (user == null || user.Deleted)
            {
                ModelState.AddModelError("InvalidCredentials", "Invalid email or password");
                ViewBag.ShowWarning = true;
                return View("Login/Index");
            }

            if (user.IsLocked)
            {
                ModelState.AddModelError("UserLocked", "User has been locked");
                ViewBag.ShowWarning = true;
                return View("Login/Index");
            }
            if (BCrypt.Net.BCrypt.Verify(formData.Password, user.Password))
            {
                // Set a cookie to indicate that the user is logged in
                HttpContext.Response.Cookies.Append("loggedIn", "true");
                ViewBag.ShowWarning = false;
                // Redirect to the home page
                return RedirectToAction("Index", "Home");
            }


            return View("Login/Index");

        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register formData)
        {
            if (!ModelState.IsValid)
                // If the model state is invalid, redisplay the form
                return View("Register/Index");

            if (!formData.Password1.Equals(formData.Password2))
                // Agregar alerta en caso que no hagan match las psw
                return View("Register/Index");

            var user = new User();
            // Call the Register method to set its properties
            user.Register(formData);
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
            // Add the user to the database
            _context.Users.Add(user);
            _context.SaveChanges();
            await _context.SaveChangesAsync();
            return RedirectToAction("login");

        }



    }
}

