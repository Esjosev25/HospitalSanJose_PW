using System;
using System.Collections.Generic;

namespace HospitalSanJoseAPI.Models;

public partial class MedicalRecord
{
    public int Id { get; set; }

    public int DoctorId { get; set; }

    public DateTime RecordDate { get; set; }

    public string Gender { get; set; } = null!;

    public int NumberOfChildren { get; set; }

    public virtual ICollection<Consultation> Consultations { get; } = new List<Consultation>();

    public virtual Doctor Doctor { get; set; } = null!;
}
