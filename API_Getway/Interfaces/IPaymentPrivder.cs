using API_Getway.Models;

namespace API_Getway.Interfaces
{
    public interface IPaymentPrivder
    {
        string Charge(string merchantId, PaymentModal paymentModal);
    }
}
