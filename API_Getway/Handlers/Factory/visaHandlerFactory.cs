using API_Getway.Handlers.PaymentHandlers;
using API_Getway.Interfaces;
using API_Getway.Models;

namespace API_Getway.Handlers.Factory
{
    public class visaHandlerFactory : IPaymentHandlerFactory

    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GeneralSettings _generalSettings;
        private readonly IDbConnector _dbConnector;

        public visaHandlerFactory(IHttpClientFactory httpClientFactory, GeneralSettings generalSettings, IDbConnector dbConnector)
        { 
            _httpClientFactory = httpClientFactory;
            _generalSettings = generalSettings;
            _dbConnector = dbConnector;
        }
        public IPaymentPrivder Create() => new VisaPaymentHandler(_httpClientFactory, _generalSettings, _dbConnector);

    }
}
