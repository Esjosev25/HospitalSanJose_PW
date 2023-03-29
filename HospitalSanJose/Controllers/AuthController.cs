
using HospitalSanJoseModel.DTO.Auth;
using HospitalSanJose.Functions;
using HospitalSanJose.Models;
using HospitalSanJoseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Extensions;

namespace HospitalSanJose.Controllers
{
  public class AuthController : Controller
    {

        private readonly HospitalDbContext _context;
        private readonly ILogger<AuthController> _logger;
        private readonly UserRolesService _userRolesService;
        private readonly AuthService _authRolesService;
        public AuthController( ILogger<AuthController> logger, AuthService authRolesService, UserRolesService userRolesService)
        {

    
            _logger = logger;
            _authRolesService = authRolesService;
             _userRolesService = userRolesService;
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
        public async Task<IActionResult> Dashboard()
        {

           var response= await SaveRolesInSession();
            if(!response)
                return RedirectToAction("Login");
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
        public async Task<IActionResult> Login(Login login)
        {
            var response = await _authRolesService.Login(login);
            if (response == null)
                return View();
            if(response.Response!=null && response.Response.ShowWarning)
                return View(response);
            HttpContext.Response.Cookies.Append("loggedIn", "true");
            HttpContext.Session.SetString("Username", login.Username);
            HttpContext.Session.SetInt32("UserId", (int)response.UserId!);
            return RedirectToAction("Index", "Dashboard");

        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register)
        {
            var response = await _authRolesService.Register(register);
            if (response == null)
                return View(response);
            if (response.Response != null && response.Response.ShowWarning)
                return View(response);
            HttpContext.Response.Cookies.Append("loggedIn", "true");
            HttpContext.Session.SetString("Username", register.Username);
            HttpContext.Session.SetInt32("UserId", (int)response.UserId!);
            return RedirectToAction("Index", "Dashboard");

        }

        private async Task<bool> SaveRolesInSession()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
           
            var userRoles = await _userRolesService.GetRolesByUserId((int)userid);
            if(userRoles == null || userRoles.Roles == null  || userRoles.Roles == "")
            {
                return false;
            }
            var roles = userRoles.Roles ?? "";
            HttpContext.Session.SetString("Roles", roles);
            return true;
        }
 
    }
}

