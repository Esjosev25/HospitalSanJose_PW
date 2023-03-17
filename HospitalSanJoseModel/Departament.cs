using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSanJoseModel
{
    public class Departament
    {
        public int Id { get; set; }

        public string DepartmentName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public Response? Response { get; set; }

    }
}
