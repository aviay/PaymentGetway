using Newtonsoft.Json;

namespace API_Getway.Models.VisaModels
{
    public class VisaPaymentModal
    {
        public VisaPaymentModal(PaymentModal input)
        {
            FullName = input.FullName;
            Number = input.CreditCardNumber;
            Expiration= input.ExpirationDate;
            Cvv = input.Cvv;
            TotalAmount = input.Amount;
        }

        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }
        [JsonProperty(PropertyName = "expiration")]
        public string Expiration { get; set; }
        [JsonProperty(PropertyName = "cvv")]
        public string Cvv { get; set; }
        [JsonProperty(PropertyName = "totalAmount")]
        public decimal TotalAmount { get; set; }
    }
}
