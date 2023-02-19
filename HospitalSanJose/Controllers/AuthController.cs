using Microsoft.AspNetCore.Mvc;

namespace HospitalSanJose.Controllers
{
    public class AuthController : Controller
    {
      

        public IActionResult Login()
        {
            return View("Login/Index");
        }

        public IActionResult Register()
        {
            return View("Register/Index");
        }
    }
}
