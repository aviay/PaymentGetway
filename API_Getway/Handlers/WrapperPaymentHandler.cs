using API_Getway.Interfaces;
using API_Getway.Models;

namespace API_Getway.Handlers
{
    public class WrapperPaymentHandler : IPayment
    {
        private readonly IPaymentHandler _paymentHandler;
        private readonly IDbConnector _dbConnector;

        public WrapperPaymentHandler(IPaymentHandler paymentHandler, IDbConnector dbConnector)
        {
            _paymentHandler = paymentHandler;
            _dbConnector = dbConnector;
        }
        public string Charge(string merchantId, string creditCardCompany, PaymentModal paymentModal)
        {
            IPaymentPrivder paymentProviderHandler = _paymentHandler.ExecuteCreation(creditCardCompany);
            return paymentProviderHandler.Charge(merchantId, paymentModal);

        }

        public IEnumerable<Charge> ChargeStatuses(string merchantId)
        {
            return _dbConnector.GetChargeStatusesFromDb(merchantId);            
        }
    }
}
