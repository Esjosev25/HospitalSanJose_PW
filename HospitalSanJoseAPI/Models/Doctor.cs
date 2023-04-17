using System;
using System.Collections.Generic;

namespace HospitalSanJoseAPI.Models;

public partial class Doctor
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Specialty { get; set; } = null!;

    public int YearsOfExperience { get; set; }

    public int Qualification { get; set; }

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual ICollection<Consultation> Consultations { get; } = new List<Consultation>();

    public virtual ICollection<DoctorDepartment> DoctorDepartments { get; } = new List<DoctorDepartment>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; } = new List<MedicalRecord>();

    public virtual ICollection<Prescription> Prescriptions { get; } = new List<Prescription>();

    public virtual User User { get; set; } = null!;
}
