using System;
using System.Collections.Generic;

namespace HospitalSanJose.Models;

public partial class PersonalInfo
{
    public int Id { get; set; }

    public string Dpi { get; set; } = null!;

    public string PhoneNumber1 { get; set; } = null!;

    public string? PhoneNumber2 { get; set; }

    public DateTime? Birthdate { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? MaritalStatus { get; set; }

    public string? City { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
