using Newtonsoft.Json;

namespace API_Getway.Models.MasterCardModals
{
    public class MasterCardPaymentModal
    {
        public MasterCardPaymentModal(PaymentModal input)
        {
            var parts = input.FullName.Split(" ");
            FirstName = parts.Length == 2 ? parts[0] : parts.FirstOrDefault();
            LastName = parts.Length == 2 ? parts[1] : string.Empty;
            CardNumber = input.CreditCardNumber;
            Expiration = GetMasterCardDateFormat(input.ExpirationDate);
            Cvv = input.Cvv;
            ChargeAmount = input.Amount;
        }

        private string? GetMasterCardDateFormat(string expirationDate)
        {
            var parts = expirationDate.Split("/");
            return $"{parts[0]}-{parts[1]}";
        }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "card_number")]
        public string CardNumber { get; set; }
        [JsonProperty(PropertyName = "expiration")]
        public string Expiration { get; set; }
        [JsonProperty(PropertyName = "cvv")]
        public string Cvv { get; set; }
        [JsonProperty(PropertyName = "charge_amount")]
        public decimal ChargeAmount { get; set; }
    }
}
