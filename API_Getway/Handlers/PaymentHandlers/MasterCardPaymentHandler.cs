using API_Getway.Interfaces;
using API_Getway.Models;
using API_Getway.Models.MasterCardModals;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System.Net;

namespace API_Getway.Handlers.PaymentHandlers
{
    public class MasterCardPaymentHandler : IPaymentPrivder
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GeneralSettings _generalSettings;
        private readonly IDbConnector _dbConnector;

        public MasterCardPaymentHandler(IHttpClientFactory httpClientFactory, GeneralSettings generalSettings, IDbConnector dbConnector)
        {
            _httpClientFactory = httpClientFactory;
            _generalSettings = generalSettings;
            _dbConnector = dbConnector;
        }

        public string Charge(string merchantId, PaymentModal paymentModal)
        {
            bool isBuisnessError = false;
            bool isChargeSuccess = false;
            var httpClient = _httpClientFactory.CreateClient();

            RetryPolicy<HttpResponseMessage> httpRetryPolicy = Policy
                        .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                        .Or<HttpRequestException>()
                        .WaitAndRetry(_generalSettings.RetryAttempt, retryAttempt =>
                                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) ));

            HttpResponseMessage httpResponseMessage = httpRetryPolicy.Execute(
                    () =>
                        {
                            MasterCardPaymentModal masterCardPaymentModal = new(paymentModal);
                            string json = JsonConvert.SerializeObject(masterCardPaymentModal);
                            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                            httpClient.DefaultRequestHeaders.Add("identifier", "Avia");
                            var response = httpClient.PostAsync(_generalSettings.MasterCardEndpoint, httpContent).GetAwaiter().GetResult();
                            var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                            if (response.StatusCode == HttpStatusCode.BadRequest)
                            {
                                var result = JsonConvert.DeserializeObject<MasterCardErrorResponse>(jsonString);
                                _dbConnector.AddDeclineReasonToDb(merchantId, result.Decline_reason);
                                isBuisnessError = true;
                            }
                            else if (response.StatusCode == HttpStatusCode.OK)
                            {
                                isChargeSuccess = true;
                            }
                            return response;
                        }

                    );
            if (!isChargeSuccess)
            {
                if (isBuisnessError)
                {
                    return "Card decline";
                }
                throw new InvalidOperationException("Visa charge failed");
            }
            return string.Empty;
        }
    }
}
