using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalSanJoseModel
{
    public class JWTResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public Response? Response { get; set; }
    }
}
