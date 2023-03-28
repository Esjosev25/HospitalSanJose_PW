using System.ComponentModel;

namespace HospitalSanJoseModel
{
    public class PersonalInfo
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [DisplayName("DPI")]
        public string Dpi { get; set; } = null!;

        [DisplayName("Telefono")]
        public string PhoneNumber { get; set; } = null!;

        [DisplayName("Telefono de Emergencias")]
        public string? EmergencyPhoneNumber { get; set; }

        [DisplayName("Fecha de Nacimiento")]
        public DateTime Birthdate { get; set; }

        [DisplayName("Dirección 1")]
        public string? AddressLine1 { get; set; }

        [DisplayName("Dirección 2")]
        public string? AddressLine2 { get; set; }

        [DisplayName("Estado Civil")]
        public string? MaritalStatus { get; set; }

        [DisplayName("Ciudad")]
        public string? City { get; set; }

        public  User? User { get; set; } = null!;

        public Response? Response { get; set; }
    }
}
