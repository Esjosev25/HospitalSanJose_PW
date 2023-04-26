namespace HospitalSanJoseAPI.Utils
{
   
    public class AttentionHours
    {
      
        public static IEnumerable<string> GetAttentionHours()
        {
            var Hours = Enumerable.Range(6, 18).Select(i => (DateTime.MinValue.AddHours(i)).ToString("HH:mm:ss"));
            return Hours;
        }
    }
}
