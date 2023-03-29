namespace HospitalSanJoseModel.DTO.Profile
{
    public class ProfileChangePassword
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public HospitalSanJoseModel.User? User { get; set; }
        public Response? Response { get; set; }
    }
}
