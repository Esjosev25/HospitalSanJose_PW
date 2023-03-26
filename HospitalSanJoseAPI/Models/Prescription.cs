using System;
using System.Collections.Generic;

namespace HospitalSanJoseAPI.Models;

public partial class Prescription
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public int UserId { get; set; }

    public int ConsultationId { get; set; }

    public DateTime PrescriptionDate { get; set; }

    public string? Instructions { get; set; }

    public virtual Consultation Consultation { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
