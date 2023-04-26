using System.ComponentModel;

namespace HospitalSanJoseModel
{
    public class Appointment
    {
        public int Id { get; set; }

        public int? DoctorId { get; set; }

        public int? UserId { get; set; }
        [DisplayName("Fecha")]
        public DateTime AppointmentDate { get; set; }
        [DisplayName("Hora")]
        public TimeSpan? AppointmentTime { get; set; }


        public  Doctor? Doctor { get; set; }

        public  User? User { get; set; }
    }
}
