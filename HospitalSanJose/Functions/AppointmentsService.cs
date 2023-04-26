using HospitalSanJoseModel;
using HospitalSanJoseModel.DTO.Appointment;

namespace HospitalSanJose.Functions
{
    public class AppointmentsService : APIServices
    {
        public AppointmentsService(IHttpContextAccessor accessor) : base(accessor) { }
        private readonly string ControllerUrl = "api/Appointments";
        public async Task<IEnumerable<Appointment>> GetList()
        {

            return await Get<IEnumerable<Appointment>>($"{ControllerUrl}");
        }

        public async Task<IEnumerable<Appointment>> GetListByDoctor(int doctorId)
        {

            return await Get<IEnumerable<Appointment>>($"{ControllerUrl}/ByDoctor/{doctorId}");
        }
        public async Task<IEnumerable<Appointment>> GetListByPacient(int userId)
        {

            return await Get<IEnumerable<Appointment>>($"{ControllerUrl}/ByPacient/{userId}");
        }
        public async Task<IEnumerable<string>> GetAttentionHours(string date, int doctorId)
        {

            return await Get<IEnumerable<string>>($"{ControllerUrl}/AttentionHours/{date}/{doctorId}");
        }


        public async Task<Appointment?> GetById(int? id)
        {

            return await Get<Appointment?>($"{ControllerUrl}/{id}");

        }
        public async Task<Appointment?> GetByUserId(int? id)
        {

            return await Get<Appointment?>($"{ControllerUrl}/User/{id}");

        }
        public async Task<AppointmentCreate> Post(AppointmentCreate appointment)
        {
            return await Post<AppointmentCreate>(appointment, $"{ControllerUrl}");
        }

        public async Task<Appointment> Put(Appointment department, int id)
        {
            return await Put(department, $"{ControllerUrl}/{id}");
        }
        public async Task Delete(int? id)
        {
            await Delete($"{ControllerUrl}/{id}");
        }

    }
}
