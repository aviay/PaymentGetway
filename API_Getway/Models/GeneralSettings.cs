namespace API_Getway.Models
{
    public class GeneralSettings
    {
        public IEnumerable<string> PaymentProviders { get; set; }
        public int RetryAttempt { get; set; }
        public string MasterCardEndpoint { get; set; }
        public string VisaEndpoint { get; set; }
    }
}
