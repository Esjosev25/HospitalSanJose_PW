using HospitalSanJoseModel;
using HospitalSanJoseModel.DTO.Profile;
using HospitalSanJoseModel.DTO.UserRoles;

namespace HospitalSanJose.Functions
{
    public class UserRolesService : APIServices
    {
        public UserRolesService(IHttpContextAccessor accessor) : base(accessor) { }
        private readonly string ControllerUrl = "api/UserRoles";
        public async Task<IEnumerable<UserRole>> GetList()
        {

            return await Get<IEnumerable<UserRole>>($"{ControllerUrl}");
        }

        public async Task<UserRolesSession> GetRolesByUserId( int userId)
        {

            return await Get<UserRolesSession>($"{ControllerUrl}/Session/ByUser/{userId}");
        }
        public async Task<IEnumerable<UserRole>> GetUserRolesById(int userId)
        {

            return await Get<IEnumerable<UserRole>>($"{ControllerUrl}/ByUser/{userId}");
        }

        public async Task<UserRolesCreate> Post(UserRolesCreate userRole)
        {
            return await Post<UserRolesCreate>(userRole, $"{ControllerUrl}");
        }

        public async Task Delete(int? id)
        {
            await Delete($"{ControllerUrl}/{id}");
        }

    }
}
