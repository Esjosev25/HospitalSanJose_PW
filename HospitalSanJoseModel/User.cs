using System.ComponentModel;

namespace HospitalSanJoseModel
{
    public class User
    {
        public int Id { get; set; }
        public string Password { get; set; } = null!;

        [DisplayName("Correo")]
        public string Email { get; set; } = null!;

        [DisplayName("Nombre")]
        public string FirstName { get; set; } = null!;

        [DisplayName("Apellido")]
        public string LastName { get; set; } = null!;

        public byte[]? Image { get; set; }
        [DisplayName("Eliminado")]
        public bool Deleted { get; set; }
        [DisplayName("Estado")]
        public bool Activated { get; set; }

        public string Username { get; set; } = null!;
        [DisplayName("Bloqueado")]
        public bool IsLocked { get; set; }

        public Response? Response { get; set; }
    }
}
