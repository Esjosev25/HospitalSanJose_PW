using HospitalSanJoseModel;
using HospitalSanJoseModel.DTO.Profile;

namespace HospitalSanJose.Functions
{
    public class RolesService : APIServices
    {
        private readonly string ControllerUrl = "api/Roles";
        public async Task<IEnumerable<Role>> GetList()
        {

            return await Get<IEnumerable<Role>>($"{ControllerUrl}");
        }

        public async Task<IEnumerable<Role>?> GetAvailableRolesForUser(int? userId)
        {

            return await Get<IEnumerable<Role>>($"{ControllerUrl}/AvailableRolesForUser/{userId}");

        }

        public async Task<Role?> GetById(int? id)
        {

            return await Get<Role?>($"{ControllerUrl}/{id}");

        }
       

    }
}
