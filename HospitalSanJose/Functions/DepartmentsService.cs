using HospitalSanJoseModel;
namespace HospitalSanJose.Functions
{
    public class DepartmentsService : APIServices
    {
        public DepartmentsService(IHttpContextAccessor accessor) : base(accessor) { }
        private readonly string ControllerUrl = "api/Departments";
        public async Task<IEnumerable<Department>> GetList()
        {

            return await Get<IEnumerable<Department>>($"{ControllerUrl}");
        }
        public async Task<IEnumerable<Department>> GetAssignedList()
        {

            return await Get<IEnumerable<Department>>($"{ControllerUrl}/AssignedDepartments");
        }
        public async Task<IEnumerable<Department>> GetAvailableDepartmentsForDoctor(int? doctorId)
        {

            return await Get<IEnumerable<Department>>($"{ControllerUrl}/AvailableDepartmentsForDoctor/{doctorId}");
        }


        public async Task<Department?> GetById(int? id)
        {

            return await Get<Department?>($"{ControllerUrl}/{id}");

        }
        public async Task<Department?> GetByUserId(int? id)
        {

            return await Get<Department?>($"{ControllerUrl}/User/{id}");

        }
        public async Task<Department> Post(Department department)
        {
            return await Post<Department>(department, $"{ControllerUrl}");
        }

        public async Task<Department> Put(Department department, int id)
        {
            return await Put(department, $"{ControllerUrl}/{id}");
        }
        public async Task Delete(int? id)
        {
            await Delete($"{ControllerUrl}/{id}");
        }

    }
}
