using HospitalSanJose.DTO;
using HospitalSanJose.Models;
using HospitalSanJose.Models.Auth;
using Microsoft.AspNetCore.Mvc;

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
            var name = HttpContext.Session.GetString("Username");
            var id = HttpContext.Session.GetInt32("UserId").ToString();
            if(name != null && id != null)
                return RedirectToAction("dashboard");
            return View("Login");
        }

        public IActionResult Register()
        {
            return View("Register");
        }
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            // var name = HttpContext.Session.GetString("Username");
            // var id = HttpContext.Session.GetInt32("UserId").ToString();
            // if (name != null && id != null)
            // return RedirectToAction("Login");
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
            {
                // If the model state is invalid, redisplay the form
                return View(userDto);
            }
            var user = _context.Users.FirstOrDefault(u => u.Username == formData.Username);

            // Look up the user in the database
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
                logInformation($"El usuario {user.Username} inició sesión");

                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("UserId", user.Id);
                // Redirect to the home page
                return RedirectToAction("Index", "Dashboard");
            }
            return View(userDto);

        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register formData)
        {
            var userDto = new UserDto
            {
                ShowWarning = true,
                AlertTitle = "Aviso",
                AlertMessage = "Ya existe un usuario con ese correo/username",
                AlertIcon = "error",
            };

            if (!ModelState.IsValid)
                // If the model state is invalid, redisplay the form
                return View(userDto);
            var userDB = _context.Users.FirstOrDefault(u => u.Username == formData.Username || u.Email == formData.Email);
            if (userDB != null && !userDB.Deleted)
                return View(userDto);

            if (!formData.Password1.Equals(formData.Password2))
            {
                // Agregar alerta en caso que no hagan match las psw
                userDto.AlertIcon = "warning";
                userDto.AlertMessage = "Ambos password deben de coincidir";
                return View(userDto);
            }
            userDto.ShowWarning = false;
            var user = new User();
            // Call the Register method to set its properties
            user.Register(formData);
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

