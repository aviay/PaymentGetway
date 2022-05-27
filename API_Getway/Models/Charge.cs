namespace API_Getway.Models
{
    public class Charge
    {
        public string Id { get; set; }

        public string MerchantId { get; set; }

        public Merchant Merchant { get; set; }

        public string Reason { get; set; }
        public int  Count { get; set; }
    }
}
