using System;
using System.Collections.Generic;

namespace HospitalSanJoseAPI.Models;

public partial class Department
{
    public int Id { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<DoctorDepartment> DoctorDepartments { get; } = new List<DoctorDepartment>();
}
