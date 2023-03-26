using System.ComponentModel;

namespace HospitalSanJoseModel
{
  public class UserUpdateDTO
    {

        public string? Password { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public bool? Deleted { get; set; }
        public bool? Activated { get; set; }
        public string? Username { get; set; } = null!;
        public bool? IsLocked { get; set; }
        public Response? Response { get; set; } 
    }
}
