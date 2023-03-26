using System;
using System.Collections.Generic;

namespace HospitalSanJoseAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public byte[]? Image { get; set; }

    public bool Deleted { get; set; }

    public bool Activated { get; set; }

    public string Username { get; set; } = null!;

    public bool IsLocked { get; set; }

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual ICollection<Consultation> Consultations { get; } = new List<Consultation>();

    public virtual ICollection<Doctor> Doctors { get; } = new List<Doctor>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; } = new List<MedicalRecord>();

    public virtual ICollection<PersonalInfo> PersonalInfos { get; } = new List<PersonalInfo>();

    public virtual ICollection<Prescription> Prescriptions { get; } = new List<Prescription>();

    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
}
