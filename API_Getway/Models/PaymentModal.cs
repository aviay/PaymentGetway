using API_Getway.Models.DTOs;

namespace API_Getway.Models
{
    public class PaymentModal
    {
        public PaymentModal(CreateChargeDTO input)
        {
            FullName = input.FullName;
            CreditCardNumber = input.CreditCardNumber;
            ExpirationDate = input.ExpirationDate;
            Cvv = input.Cvv;
            Amount = input.Amount;
        }

        public string FullName { get; set; }
        public string CreditCardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string Cvv { get; set; }
        public decimal Amount { get; set; }
    }
}
