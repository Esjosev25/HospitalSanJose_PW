namespace HospitalSanJoseModel.DTO.Auth
{
    public partial class Login
    {
        public int? UserId { get; set; }
        public string Password { get; set; } = null!;
        public string Username { get; set; } = null!;
        public Response? Response { get; set; }
    }
}
