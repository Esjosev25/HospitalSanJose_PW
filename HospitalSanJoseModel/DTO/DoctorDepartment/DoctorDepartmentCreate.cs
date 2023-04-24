using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HospitalSanJoseModel;

namespace HospitalSanJoseModel.DTO.DoctorDepartment
{
    public class DoctorDepartmentCreate
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public int DepartmentId { get; set; }

        public HospitalSanJoseModel.Doctor Doctor { get; set; } = null!;

        public HospitalSanJoseModel.Department Department { get; set; } = null!;
        public List<SelectListItem>? Doctors { get; set; }
        public List<SelectListItem>? Departments { get; set; }
        public Response? Response { get; set; }
    }
}
