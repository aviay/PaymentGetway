using API_Getway.Models;

namespace API_Getway.Interfaces
{
    public interface IPayment
    {
        string Charge(string merchantId, string creditCardCompany, PaymentModal paymentModal);
        IEnumerable<Charge> ChargeStatuses(string merchantId);
    }
}
