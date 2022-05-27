using API_Getway.Models.DTOs;

namespace API_Getway.Interfaces
{
    public interface IValidation
    {
        bool IsValidDateFormat(string date);
        bool IsValidPaymentProvider(string date);
        void ValidateChargeInput(string merchantId, CreateChargeDTO input);
    }
}
