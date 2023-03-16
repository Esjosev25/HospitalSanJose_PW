namespace HospitalSanJose.DTO
{
    public partial class Login
    {
        public string Password { get; set; } = null!;
        public string Username { get; set; } = null!;

        public HospitalSanJoseModel.Response? Response { get; set; }
    }
}
