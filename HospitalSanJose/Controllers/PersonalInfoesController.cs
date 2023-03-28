using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalSanJoseModel;
using HospitalSanJose.Functions;

namespace HospitalSanJose.Controllers
{
    public class PersonalInfoesController : Controller
    {
        private readonly PersonalInfosService _personalInfoService;
        private readonly UsersService _usersService;
        public PersonalInfoesController( PersonalInfosService personalInfosService, UsersService usersService)
        {
            
            _personalInfoService = personalInfosService;
            _usersService = usersService;
        }

        // GET: PersonalInfoes
        public async Task<IActionResult> Index()
        {
            var personalInfos = await _personalInfoService.GetList();
            return View(personalInfos);
        }

        // GET: PersonalInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var personalInfo = await _personalInfoService.GetById(id);
            if (personalInfo.Id == 0)
            {
                return NotFound();
            }

            return View(personalInfo);
        }

        // GET: PersonalInfoes/Create
        public async Task<IActionResult> Create(int? Id)
        {

            var users = await CreatePI(Id);
            if (users == null)
                return NotFound();
            ViewData["UserId"] = new SelectList(users, "Id", "Username");
            return View();
        }
        async Task<List<User>> CreatePI(int? Id)
        {
            var users = await _usersService.GetListInactiveUsers(Id);
            return (List<User>)users;
        }
        // POST: PersonalInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Dpi,PhoneNumber,EmergencyPhoneNumber,Birthdate,AddressLine1,AddressLine2,MaritalStatus,City")] PersonalInfo personalInfo)
        {

            var  response = await _personalInfoService.Post(personalInfo);
            if (response != null && response.Response != null)
            {
                var users = await CreatePI(0);
                return View(response);
            }
            return RedirectToAction(nameof(Index));


        }

        // GET: PersonalInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var personalInfo = await _personalInfoService.GetById(id);
            if (personalInfo.Id == 0)
            {
                return NotFound();
            }

            return View(personalInfo);
        }

        // POST: PersonalInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Dpi,PhoneNumber,EmergencyPhoneNumber,Birthdate,AddressLine1,AddressLine2,MaritalStatus,City")] PersonalInfo personalInfo)
        {
            if (!ModelState.IsValid)
                return  View(personalInfo);
            var response = await _personalInfoService.Put(personalInfo, id);
            if (response != null)
            {
                return View(response);

            }
            return RedirectToAction(nameof(Index));

        }

        // GET: PersonalInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            var personalInfo = await _personalInfoService.GetById(id);
            if (personalInfo.Id == 0)
            {
                return NotFound();
            }

            return View(personalInfo);
        }

        // POST: PersonalInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _personalInfoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditByUserID(int id)
        {
           
           var personalInfo = await _personalInfoService.GetByUserId(id);
            if (personalInfo.Id != 0)
                return Redirect($"/PersonalInfoes/Edit/{personalInfo.Id}");

            return RedirectToAction(nameof(Create));
        }


    }
}
