using API_Getway.Interfaces;
using API_Getway.Models.DTOs;
using System.Text.RegularExpressions;

namespace API_Getway.Handlers
{
    public class ValidationHandler : IValidation
    {
        public bool IsValidDateFormat(string date)
        {
            try
            {
                var rgx = new Regex(@"^(0[1-9]{1}|1[0-2]{1})/\d{2}$");
                return rgx.IsMatch(date);
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidPaymentProvider(string date)
        {
            throw new NotImplementedException();
        }

       

        public void ValidateChargeInput(string merchantId, CreateChargeDTO input)
        {
            if (string.IsNullOrWhiteSpace(merchantId))
            {
                throw new InvalidOperationException("invalid merchantId");
            }
            if(string.IsNullOrWhiteSpace(input.FullName) ||
                string.IsNullOrWhiteSpace(input.CreditCardNumber) ||
                string.IsNullOrWhiteSpace(input.Cvv) ||
                string.IsNullOrWhiteSpace(input.ExpirationDate) ||
                string.IsNullOrWhiteSpace(input.CreditCardCompany))
            {
                throw new InvalidOperationException("missing input parameters");
            }
            if (input.Amount < 1)
            {
                throw new InvalidOperationException("invalid Amount parameter");
            }
            if (!IsValidDateFormat(input.ExpirationDate))
            {
                throw new InvalidOperationException("invalid expiration");
            }


        }
    }
}
