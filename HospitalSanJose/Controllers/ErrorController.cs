using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalSanJose.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error")]
        public IActionResult Index()
        {
            return View("Error");
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            //Request.RouteValues.Last().ToString();
            switch (statusCode)
            {
                case 404:
                    return View("NotFound");
                default:
                    return View("Error");
            }
        }
    }
}
