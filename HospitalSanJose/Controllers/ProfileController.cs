using AutoMapper;
using Azure;
using HospitalSanJose.Functions;
using HospitalSanJoseModel;
using HospitalSanJoseModel.DTO.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalSanJose.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IMapper _mapper;

        private readonly UsersService _usersService;
        private readonly PersonalInfosService _personalInfosService;

        public ProfileController(UsersService usersService, PersonalInfosService personalInfosService, IMapper mapper)
        {

            _mapper = mapper;
            _usersService = usersService;
            _personalInfosService = personalInfosService;
        }

        // GET: Users/Profile
        public async Task<IActionResult> Index()
        {
            ViewBag.ActiveTab = "profile-overview";

            var personalInfo = await GetPersonalInfo();
            if (personalInfo == null)
            {
                return RedirectToAction("401", "Error"); // redirect to the error page
            }
            return View(personalInfo);
        }

        // GET: Profile/Edit/5
        public async Task<IActionResult> Edit()
        {
            ViewBag.ActiveTab = "profile-edit";

            var personalInfo = await GetPersonalInfo();
            if (personalInfo == null)
            {
                return RedirectToAction("401", "Error"); // redirect to the error page
            }
            return View(personalInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PersonalInfo personalInfo, int piId)
        {
            ViewBag.ActiveTab = "profile-edit";

            var id = HttpContext.Session.GetInt32("UserId");
            var personalInfoDb = await _personalInfosService.GetByUserId(id);

            var userDB = await _usersService.GetById(id);
            if (userDB == null)
            {
                return NotFound();
            }

            userDB.Email = personalInfo.User.Email;
            userDB.Username = personalInfo.User.Username;
            var responseUser = await _usersService.Put(userDB, (int)id);
            if (responseUser != null && responseUser.Response != null)
            {
                personalInfo.User.FirstName = userDB.FirstName;
                personalInfo.User.LastName = userDB.LastName;
                personalInfo.User.Activated = userDB.Activated;
                personalInfo.User.Email = responseUser.Email;
                personalInfo.Response = responseUser.Response;
                return View(personalInfo);
            }

            personalInfoDb.PhoneNumber = personalInfo.PhoneNumber;
            personalInfoDb.EmergencyPhoneNumber = personalInfo.EmergencyPhoneNumber;
            personalInfoDb.MaritalStatus = personalInfo.MaritalStatus;
            personalInfoDb.City = personalInfo.City;
            personalInfoDb.AddressLine1 = personalInfo.AddressLine1;
            personalInfoDb.AddressLine2 = personalInfo.AddressLine2;
            var responsePI = await _personalInfosService.Put(personalInfoDb, personalInfoDb.Id);
            if (responsePI != null && responsePI.Response != null)
            {
                personalInfo.User.FirstName = userDB.FirstName;
                personalInfo.User.LastName = userDB.LastName;
                personalInfo.User.Email = responseUser.Email;
                personalInfo.Dpi = responsePI.Dpi;
                personalInfo.Birthdate = responsePI.Birthdate;
                personalInfo.User.Activated = userDB.Activated;
                personalInfo.Response = responsePI.Response;
                return View(personalInfo);
            }
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> ChangePassword()
        {
            ViewBag.ActiveTab = "profile-change-password";

            var personalInfo = _mapper.Map<ProfileChangePassword>(await GetPersonalInfo());
            if (personalInfo == null)
            {
                return RedirectToAction("401", "Error"); // redirect to the error page
            }
            return View(personalInfo);
        }


        async Task<PersonalInfo> GetPersonalInfo()
        {
            var id = HttpContext.Session.GetInt32("UserId");

            var user = await _usersService.GetById(id);


            var personalInfo = await _personalInfosService.GetByUserId(id);
            if (personalInfo == null || personalInfo.Id == 0)
                personalInfo = new PersonalInfo
                {
                    User = user
                };
            else
                personalInfo.User = user;
            return personalInfo;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([Bind("OldPassword,NewPassword,ConfirmPassword")] ProfileChangePassword changePassword)
        {
            ViewBag.ActiveTab = "profile-edit";

            var id = HttpContext.Session.GetInt32("UserId");

            var response = await _usersService.ChangePassword(changePassword, (int)id);
            if (response != null)
            {
                return View(response);
            }
            return RedirectToAction(nameof(Index));

        }

    }
}
