using API_Getway.Handlers.PaymentHandlers;
using API_Getway.Interfaces;
using API_Getway.Models;

namespace API_Getway.Handlers.Factory
{
    public class mastercardHandlerFactory : IPaymentHandlerFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GeneralSettings _generalSettings;
        private readonly IDbConnector _dbConnector;

        public mastercardHandlerFactory(IHttpClientFactory httpClientFactory, GeneralSettings generalSettings, IDbConnector dbConnector)
        {
            _httpClientFactory = httpClientFactory;
            _generalSettings = generalSettings;
            _dbConnector = dbConnector;
        }

        public IPaymentPrivder Create() => new MasterCardPaymentHandler(_httpClientFactory, _generalSettings, _dbConnector);

    }
}
