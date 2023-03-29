using HospitalSanJoseModel;
using HospitalSanJoseModel.DTO.Profile;
using HospitalSanJoseModel.DTO.UserRoles;

namespace HospitalSanJose.Functions
{
    public class UserRolesService : APIServices
    {
        private readonly string ControllerUrl = "api/UserRoles";
        public async Task<IEnumerable<UserRole>> GetList()
        {

            return await Get<IEnumerable<UserRole>>($"{ControllerUrl}");
        }

        public async Task<UserRolesSession> GetRolesByUserId( int userId)
        {

            return await Get<UserRolesSession>($"{ControllerUrl}/ByUser/{userId}");
        }


        public async Task<User> Post(User user)
        {
            return await Post(user, $"{ControllerUrl}");
        }

        public async Task Delete(int? id)
        {
            await Delete($"{ControllerUrl}/{id}");
        }

    }
}
