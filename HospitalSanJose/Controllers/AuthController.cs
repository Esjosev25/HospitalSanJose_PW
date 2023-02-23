using HospitalSanJose.DTO;
using HospitalSanJose.Models;
using HospitalSanJose.Models.Auth;
using Microsoft.AspNetCore.Mvc;

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

            return View("Login");
        }

        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login formData)
        {
            var userDto = new UserDto
            {
                ShowWarning = true,
                AlertTitle = "Aviso",
                AlertMessage = "Error en iniciar sesion",
                AlertIcon = "error",
            };

            if (!ModelState.IsValid)
                // If the model state is invalid, redisplay the form
                return View(userDto);

            // Look up the user in the database
            var user = _context.Users.FirstOrDefault(u => u.Username == formData.Username);

            if (user == null || user.Deleted)
            {
                userDto.AlertIcon = "error";

                return View(userDto);
            }

            if (user.IsLocked)
            {
                userDto.AlertMessage = "Usuario bloqueado";
                return View(userDto);
            }


            if (BCrypt.Net.BCrypt.Verify(formData.Password, user.Password))
            {
                // Set a cookie to indicate that the user is logged in
                HttpContext.Response.Cookies.Append("loggedIn", "true");
                userDto.ShowWarning = false;
                // Redirect to the home page
                return RedirectToAction("Index", "Home");
            }



            return View(userDto);

        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register formData)
        {
            ViewBag.ShowWarning = true;
            ViewBag.alertTitle = "Aviso";
            ViewBag.AlertMessage = "Usuario ya existente";
            ViewBag.alertIcon = "error";
            if (!ModelState.IsValid)
                // If the model state is invalid, redisplay the form
                return View("Register");
            var userDB = _context.Users.FirstOrDefault(u => u.Username == formData.Username || u.Email == formData.Email);
            if (userDB != null)
                return View("Register");

            if (!formData.Password1.Equals(formData.Password2))
            {
                // Agregar alerta en caso que no hagan match las psw
                ViewBag.alertIcon = "warning";
                ViewBag.AlertMessage = "Tus contraseñas deben de coincidir";
                return View("Register");
            }
            ViewBag.ShowWarning = false;
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

