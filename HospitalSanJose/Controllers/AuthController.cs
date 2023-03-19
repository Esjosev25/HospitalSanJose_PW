using HospitalSanJose.DTO;
using HospitalSanJose.Models;
using HospitalSanJoseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HospitalSanJose.Controllers
{
    public class AuthController : Controller
    {

        private readonly HospitalDbContext _context;
        private readonly ILogger<AuthController> _logger;
        public AuthController(HospitalDbContext context, ILogger<AuthController> logger)
        {

            _context = context;
            _logger = logger;
        }

        public IActionResult Login()
        {

            return View("Login");
        }

        public IActionResult Register()
        {
            return View("Register");
        }
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            var userid =HttpContext.Session.GetInt32("UserId");
            var username = HttpContext.Session.GetString("Username");
            var roles = (from ur in _context.UserRoles
                         join r in _context.Roles on ur.RoleId equals r.Id
                         where ur.UserId == userid
                         select r.Name).ToList();

            ViewData["Roles"] = roles;

            return View("Dashboard");
        }



        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login login)
        {
            var response = new HospitalSanJoseModel.Response();
            login.Response = response;
            if (!ModelState.IsValid)
            {
                // If the model state is invalid, redisplay the form
                return View(login);
            }
            var user = _context.Users.FirstOrDefault(u => u.Username == login.Username);

            // Look up the user in the database
            if (user == null || user.Deleted)
            {
                login.Response.AlertIcon = "error";
                login.Response.AlertMessage = "Error en iniciar sesion";
                return View(login);
            }

            if (user.IsLocked)
            {
                login.Response.AlertMessage = "Usuario bloqueado";
                return View(login);
            }

            if (BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                // Set a cookie to indicate that the user is logged in
                HttpContext.Response.Cookies.Append("loggedIn", "true");
                login.Response.ShowWarning = false;
                logInformation($"El usuario {user.Username} inició sesión");

                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("UserId", user.Id);

                 
                // Redirect to the home page
                return RedirectToAction("Index", "Dashboard");
            }
            return View(login);

        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register)
        {
            
            var response = new Response{
              AlertMessage = "Ya existe un usuario con ese correo/username",
              AlertIcon = "error",
            };
            register.Response = response;
            if (!ModelState.IsValid)
                // If the model state is invalid, redisplay the form
                return View(register);
            register.Email = register.Email.Trim();
            register.Username = register.Username.Trim();
            var userDB = _context.Users.FirstOrDefault(u => u.Username == register.Username || u.Email == register.Email);
            if (userDB != null)
                return View(register);

            if (!register.Password1.Equals(register.Password2))
            {
                // Agregar alerta en caso que no hagan match las psw
                register.Response.AlertIcon = "warning";
                register.Response.AlertMessage = "Ambos password deben de coincidir";
                return View(register);
            }
            register.Response.ShowWarning = false;
            var user = new Models.User{
                Password = register.Password1,
                Email = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName,
                Username = register.Username,
            };
            // Call the Register method to set its properties
            

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
            // Add the user to the database
            _context.Users.Add(user);
            _context.SaveChanges();
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Se registró el usuario {user.Username} con el correo {user.Email}");
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetInt32("UserId", user.Id);

            return RedirectToAction("dashboard");

        }


        private void logInformation(string Message)
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

            // Get the current time in the GMT-6 timezone
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);
            _logger.LogInformation($"[{GetType().Name}] [{cstTime}]: {Message}");
        }
    }
}

