namespace HospitalSanJoseModel
{
  public class Response
    {
        public bool ShowWarning { get; set; }

        public string? AlertMessage { get; set; }
        public string? AlertIcon { get; set; }

        public string? AlertTitle { get; set; }

        public Response()
        {
            AlertMessage = "";
            AlertTitle = "Advertencia";
            AlertIcon = "warning";
            ShowWarning = true;
            
        }
    }
}
