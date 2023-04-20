using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSanJoseModel
{
    public class Doctor
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [DisplayName("Especialidades")]
        public string Specialty { get; set; } = null!;
        [DisplayName("Años de Experiencia")]
        public int YearsOfExperience { get; set; }
        [DisplayName("Calificación")]
        public int Qualification { get; set; }
        public User User { get; set; } = null!;
        public Response? Response { get; set; }
    }
}
