using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HospitalSanJose.Models;

public partial class PersonalInfo
{
        
    public int Id { get; set; }
    public int UserId { get; set; }
    [DisplayName("DPI")]
    public string Dpi { get; set; } = null!;
    [DisplayName("Numero de Telefono")]
    public string PhoneNumber1 { get; set; } = null!;
    [DisplayName("Numero de Emergencias")]
    public string? PhoneNumber2 { get; set; }
    [DisplayName("Fecha de Nacimiento")]
    public DateTime? Birthdate { get; set; }
    [DisplayName("Dirección 1")]
    public string? AddressLine1 { get; set; }
    [DisplayName("Dirección 2")]
    public string? AddressLine2 { get; set; }
    [DisplayName("Estado Civil")]
    public string? MaritalStatus { get; set; }
    [DisplayName("Ciudad")]
    public string? City { get; set; }

    public virtual User User { get; set; } = null!;
}
