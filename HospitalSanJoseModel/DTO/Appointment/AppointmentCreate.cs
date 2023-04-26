using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSanJoseModel.DTO.Appointment
{
    public class AppointmentCreate
    {

        public int? DoctorId { get; set; }

        public int? UserId { get; set; }
        public int? DepartmentId { get; set; }
        [DisplayName("Fecha")]
        public DateTime AppointmentDate { get; set; }
        [DisplayName("Hora")]
        public string AppointmentTime { get; set; }


        public HospitalSanJoseModel.Doctor? Doctor { get; set; }

        public HospitalSanJoseModel.User? User { get; set; }
        public List<SelectListItem>? Users{ get; set; }
        public List<SelectListItem>? Doctors { get; set; }
        public List<SelectListItem>? Departments { get; set; }
        public List<SelectListItem>? AvailableHours{ get; set; }
        public Response? Response { get; set; }
    }
}
