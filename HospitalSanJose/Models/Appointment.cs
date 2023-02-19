using System;
using System.Collections.Generic;

namespace HospitalSanJose.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public int? DoctorId { get; set; }

    public int? UserId { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public TimeSpan? AppointmentTime { get; set; }

    public virtual ICollection<Consultation> Consultations { get; } = new List<Consultation>();

    public virtual Doctor? Doctor { get; set; }

    public virtual User? User { get; set; }
}
