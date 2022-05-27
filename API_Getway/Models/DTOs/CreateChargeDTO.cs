using System.ComponentModel.DataAnnotations;

namespace API_Getway.Models.DTOs
{
    public class CreateChargeDTO
    {
        public string FullName { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardCompany { get; set; }
        public string ExpirationDate { get; set; }
        public string Cvv { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
