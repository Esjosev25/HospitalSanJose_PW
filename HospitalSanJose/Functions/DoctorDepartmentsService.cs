using HospitalSanJoseModel;
using HospitalSanJoseModel.DTO.DoctorDepartment;

namespace HospitalSanJose.Functions
{
    public class DoctorDepartmentsService : APIServices
    {
        public DoctorDepartmentsService(IHttpContextAccessor accessor) : base(accessor) { }
        private readonly string ControllerUrl = "api/DoctorDepartments";
        public async Task<IEnumerable<DoctorDepartment>> GetList()
        {

            return await Get<IEnumerable<DoctorDepartment>>($"{ControllerUrl}");
        }

    
        public async Task<IEnumerable<DoctorDepartment>> GetDoctorDepartmentsById(int userId)
        {

            return await Get<IEnumerable<DoctorDepartment>>($"{ControllerUrl}/ByUser/{userId}");
        }
        public async Task<IEnumerable<DoctorDepartment>> GetByDoctorId(int doctorId)
        {

            return await Get<IEnumerable<DoctorDepartment>>($"{ControllerUrl}/ByDoctorId/{doctorId}");
        }
        public async Task<DoctorDepartmentCreate> Post(DoctorDepartmentCreate userRole)
        {
            return await Post<DoctorDepartmentCreate>(userRole, $"{ControllerUrl}");
        }

        public async Task Delete(int? id)
        {
            await Delete($"{ControllerUrl}/{id}");
        }

    }
}
