using HospitalSanJoseModel;

namespace HospitalSanJose.Functions
{
    public class DoctorsService : APIServices
    {
        public DoctorsService(IHttpContextAccessor accessor) : base(accessor) { }
        private readonly string ControllerUrl = "api/Doctors";
        public async Task<IEnumerable<Doctor>> GetList()
        {

            return await Get<IEnumerable<Doctor>>($"{ControllerUrl}");
        }

        public async Task<Doctor?> GetById(int? id)
        {

            return await Get<Doctor?>($"{ControllerUrl}/{id}");

        }
        public async Task<Doctor> Post(Doctor doctor)
        {
            return await Post<Doctor>(doctor, $"{ControllerUrl}");
        }

        public async Task<Doctor> Put(Doctor doctor, int id)
        {
            return await Put(doctor, $"{ControllerUrl}/{id}");
        }

        public async Task Delete(int? id)
        {
            await Delete($"{ControllerUrl}/{id}");
        }

     
    }
}
