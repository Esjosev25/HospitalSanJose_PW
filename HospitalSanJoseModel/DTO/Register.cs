namespace HospitalSanJoseModel.DTO
{
    public partial class Register
    {
        public string Password1 { get; set; } = null!;
        public string Password2 { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
        public HospitalSanJoseModel.Response? Response { get; set; }   
    }
}
