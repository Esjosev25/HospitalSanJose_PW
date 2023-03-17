using System;
using System.Collections.Generic;

namespace HospitalSanJose.Models;

public partial class Doctor
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int DepartmentId { get; set; }
    public virtual Department Department { get; set; } = null!;
    public virtual User User { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual ICollection<Consultation> Consultations { get; } = new List<Consultation>();


    public virtual ICollection<DoctorsInfo> DoctorsInfos { get; } = new List<DoctorsInfo>();

    public virtual ICollection<Prescription> Prescriptions { get; } = new List<Prescription>();

}
