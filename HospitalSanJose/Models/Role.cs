using System;
using System.Collections.Generic;

namespace HospitalSanJose.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<UserFunction> UserFunctions { get; } = new List<UserFunction>();

    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
}
