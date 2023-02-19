using System;
using System.Collections.Generic;

namespace HospitalSanJose.Models;

public partial class Consultation
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public int UserId { get; set; }

    public int AppointmentId { get; set; }

    public int MedicalRecordsId { get; set; }

    public DateTime ConsultationDate { get; set; }

    public int DurationMinutes { get; set; }

    public string? Diagnosis { get; set; }

    public string? Treatment { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual MedicalRecord MedicalRecords { get; set; } = null!;

    public virtual ICollection<Prescription> Prescriptions { get; } = new List<Prescription>();

    public virtual User User { get; set; } = null!;
}
