using System;
using System.Collections.Generic;

namespace HospitalSanJoseAPI.Models;

public partial class DoctorsInfo
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public string Specialty { get; set; } = null!;

    public int YearsOfExperience { get; set; }

    public string Qualification { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;
}
