namespace HospitalSanJoseAPI.Utils
{
   
    public class Roles
    {
        public enum RolesType
        {
            Admin,
            Secretario,
            Paciente,
            Doctor
        }
        public static IEnumerable<RolesType> GetRoles()
        {
            return Enum.GetValues(typeof(RolesType)).Cast<RolesType>();
        }
    }
}
