using Microsoft.AspNetCore.Mvc;
using HospitalSanJose.Functions;

namespace HospitalSanJose.Controllers
{
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
