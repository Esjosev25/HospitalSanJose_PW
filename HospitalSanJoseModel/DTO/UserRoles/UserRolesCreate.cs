using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSanJoseModel.DTO.UserRoles
{
    public class UserRolesCreate
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; } = null!;

        public HospitalSanJoseModel.User User { get; set; } = null!;
        public List<SelectListItem>? Users { get; set; }
        public List<SelectListItem>? Roles { get; set; }
        public Response? Response { get; set; }
    }
}
