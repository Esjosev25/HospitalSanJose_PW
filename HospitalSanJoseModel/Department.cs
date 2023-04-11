using System.ComponentModel;

namespace HospitalSanJoseModel
{
    public class Department
    {
        public int Id { get; set; }
        [DisplayName("Departamento")]
        public string DepartmentName { get; set; } = null!;
        [DisplayName("Descripcion")]
        public string Description { get; set; } = null!;

        public Response? Response { get; set; }

    }
}
