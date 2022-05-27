using API_Getway.Interfaces;
using API_Getway.Models;
using Microsoft.Extensions.Options;

namespace API_Getway.Handlers
{
    public class FactoryPaymentInitilazerHandler : IPaymentHandler
    {
        private readonly IDictionary<string, IPaymentHandlerFactory> _factories;

        public FactoryPaymentInitilazerHandler(IOptions<GeneralSettings> generalSettings, IHttpClientFactory httpClientFactory, IDbConnector dbConnector)
        {
            _factories = new Dictionary<string, IPaymentHandlerFactory>();
            foreach (var provider in generalSettings.Value.PaymentProviders)
            {
                var type = Type.GetType($" API_Getway.Handlers.Factory.{provider}HandlerFactory");
                var parameters = new object[3];
                parameters[0] = httpClientFactory;
                parameters[1] = generalSettings.Value;
                parameters[2] = dbConnector;
                var factory = (IPaymentHandlerFactory)Activator.CreateInstance(type,parameters);
                _factories.Add(provider, factory);
            }

        }
        public IPaymentPrivder ExecuteCreation(string paymentProvider)
        {
            if (!_factories.ContainsKey(paymentProvider))
            {
                throw new InvalidOperationException($"[{paymentProvider}] is not supported payment provider");
            }
            return _factories[paymentProvider].Create();
        }
    }
}
