using System.ComponentModel;

namespace HospitalSanJoseModel
{
  public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
        public Response? Response { get; set; }
    }
}
