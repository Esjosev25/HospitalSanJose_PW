using System.ComponentModel;

namespace HospitalSanJoseModel
{
  public class Role : Response
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
