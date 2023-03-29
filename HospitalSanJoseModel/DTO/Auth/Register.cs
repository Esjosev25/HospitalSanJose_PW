namespace HospitalSanJoseModel.DTO.Auth
{
    public  class Register
    {
        public int? UserId { get; set; }
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
        public Response? Response { get; set; }   
    }
}
