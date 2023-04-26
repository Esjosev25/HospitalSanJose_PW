using Microsoft.AspNetCore.Mvc;
using HospitalSanJose.Functions;
using Microsoft.AspNetCore.Authorization;

namespace HospitalSanJose.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {

        private readonly RolesService _rolesService;
        private readonly UsersService _usersService;
        public RolesController(RolesService rolesService, UsersService usersService)
        {
            _rolesService = rolesService;
            _usersService = usersService;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            var roles = await _rolesService.GetList();
            if (roles == null)
            {
                return RedirectToAction("401", "Error"); // redirect to the error page
            }
            return View(roles);
        }


        [HttpPost]
        public async Task<JsonResult> GetAvailableRolesForUserJson()
        {

            int userId = Convert.ToInt32(HttpContext.Request.Form["userId"].FirstOrDefault().ToString());
            var roles = await _rolesService.GetAvailableRolesForUser(userId);
            
            return Json(roles);
        }




    }
}
