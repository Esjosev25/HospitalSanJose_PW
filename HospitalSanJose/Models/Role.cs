using System;
using System.Collections.Generic;

namespace HospitalSanJose.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<RoleFunction> RoleFunctions { get; } = new List<RoleFunction>();

    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
}
