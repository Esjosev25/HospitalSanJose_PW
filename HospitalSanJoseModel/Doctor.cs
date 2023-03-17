using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSanJoseModel
{
    public class Doctor
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int DepartmentId { get; set; }
        public Departament Department { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
