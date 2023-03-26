using System;
using System.Collections.Generic;

namespace HospitalSanJoseAPI.Models;

public partial class PersonalInfo
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Dpi { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? EmergencyPhoneNumber { get; set; }

    public DateTime? Birthdate { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? MaritalStatus { get; set; }

    public string? City { get; set; }

    public virtual User User { get; set; } = null!;
}
