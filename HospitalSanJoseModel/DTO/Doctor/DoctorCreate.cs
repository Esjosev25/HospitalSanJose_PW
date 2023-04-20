using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace HospitalSanJoseModel.DTO.Doctor
{
    public class DoctorCreate
    {
      

        public int UserId { get; set; }
        public string Specialty { get; set; } = null!;

        public int YearsOfExperience { get; set; }

        public int Qualification { get; set; }
        public HospitalSanJoseModel.User User { get; set; } = null!;
        public List<SelectListItem>? Users { get; set; }
        public Response? Response { get; set; }
    }
}
