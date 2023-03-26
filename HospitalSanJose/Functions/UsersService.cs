using HospitalSanJoseModel;
namespace HospitalSanJoseModel.Functions
{
    public class UsersService : APIServices
    {
        private readonly string ControllerUrl = "api/Users";
        public async Task<IEnumerable<User>> GetList()
        {

            return await Get<IEnumerable<User>>($"{ControllerUrl}");
        }


        public async Task<User> GetById(int? id)
        {

            return await Get<User>($"{ControllerUrl}/{id}");

        }
        public async Task<User> Post(User user)
        {
            return await Post(user, $"{ControllerUrl}");
        }

        public async Task<User> Put(User user, int id)
        {
            return await Put(user, $"{ControllerUrl}/{id}");
        }
        public async Task Delete(int? id)
        {
            await Delete($"{ControllerUrl}/{id}");
        }

        public async Task ToggleBlockUser(int? id)
        {
            await Put($"{ControllerUrl}/Block/{id}");
        }
    }
}
