using Newtonsoft.Json;

namespace API_Getway.Models.MasterCardModals
{
    public class MasterCardErrorResponse
    {
        public string Decline_reason { get; set; }
        public string Error { get; set; }
    }
}
