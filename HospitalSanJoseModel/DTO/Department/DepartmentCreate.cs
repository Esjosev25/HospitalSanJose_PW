using System.ComponentModel;

namespace HospitalSanJoseModel.DTO.Department
{
    public class DepartmentCreate
    {
        
        public string DepartmentName { get; set; } = null!;
        
        public string Description { get; set; } = null!;
    }
}
