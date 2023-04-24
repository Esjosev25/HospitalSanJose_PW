using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSanJoseModel
{
    public class DoctorDepartment
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public int DepartmentId { get; set; }

        public  Department Department { get; set; } = null!;

        public  Doctor Doctor { get; set; } = null!;
        public Response? Response { get; set; }
    }
}
