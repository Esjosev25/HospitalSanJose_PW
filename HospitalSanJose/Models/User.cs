using HospitalSanJose.Models.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HospitalSanJose.Models;

public partial class User
{
    public User()
    {
        // Empty constructor
    }
    public void Register(Register register)
    {
        this.Password = register.Password1;
        this.Email = register.Email;
        this.FirstName = register.FirstName;
        this.LastName = register.LastName;
        this.Username = register.Username;
    }

    public int Id { get; set; }
    public string Password { get; set; } = null!;

    [DisplayName("Cambio de Contraseña")]
    public bool? NeedChangePassword { get; set; }

    [DisplayName("Correo")]
    public string Email { get; set; } = null!;

    [DisplayName("Nombre")]
    public string FirstName { get; set; } = null!;

    [DisplayName("Apellido")]
    public string LastName { get; set; } = null!;

    public byte[]? Image { get; set; }
    [DisplayName("Eliminado")]
    public bool Deleted { get; set; }
    [DisplayName("Estado")]
    public bool Activated { get; set; }

    public string Username { get; set; } = null!;
    [DisplayName("Bloqueado")]
    public bool IsLocked { get; set; }

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual ICollection<Consultation> Consultations { get; } = new List<Consultation>();

    public virtual ICollection<Doctor> Doctors { get; } = new List<Doctor>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; } = new List<MedicalRecord>();

    public virtual ICollection<PersonalInfo> PersonalInfos { get; } = new List<PersonalInfo>();

    public virtual ICollection<Prescription> Prescriptions { get; } = new List<Prescription>();

    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
}
