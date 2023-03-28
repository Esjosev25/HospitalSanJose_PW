using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSanJoseModel.DTO.PersonalInfo
{
    public class PersonalInfoCreate
    {
        
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

        public Response? Response { get; set; }
    }
}
