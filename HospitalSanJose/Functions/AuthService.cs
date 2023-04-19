using HospitalSanJoseModel;
using HospitalSanJoseModel.DTO.Auth;
using HospitalSanJoseModel.DTO.Profile;

namespace HospitalSanJose.Functions
{
    public class AuthService : APIServices
    {
        public AuthService(IHttpContextAccessor accessor) : base(accessor) { }

        private readonly string ControllerUrl = "api/Auth";


        public async Task<JWTResponse> Login(Login login)
        {
            return await Post<JWTResponse>(login, $"{ControllerUrl}/Login");
        }
        public async Task<JWTResponse> Register(Register register)
        {
            return await Post<JWTResponse>(register, $"{ControllerUrl}/Register");
        }

      
      

    }
}
