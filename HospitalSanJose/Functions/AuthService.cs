using HospitalSanJoseModel;
using HospitalSanJoseModel.DTO.Auth;
using HospitalSanJoseModel.DTO.Profile;

namespace HospitalSanJose.Functions
{
    public class AuthService : APIServices
    {
        private readonly string ControllerUrl = "api/Auth";


        public async Task<Login> Login(Login login)
        {
            return await Post(login, $"{ControllerUrl}/Login");
        }
        public async Task<Register> Register(Register register)
        {
            return await Post(register, $"{ControllerUrl}/Register");
        }

      
      

    }
}
