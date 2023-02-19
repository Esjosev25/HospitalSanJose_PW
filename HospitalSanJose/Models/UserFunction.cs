using System;
using System.Collections.Generic;

namespace HospitalSanJose.Models;

public partial class UserFunction
{
    public int Id { get; set; }

    public int FunctionId { get; set; }

    public int RoleId { get; set; }

    public virtual Function Function { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
