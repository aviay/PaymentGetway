using API_Getway.Interfaces;
using API_Getway.Models;
using API_Getway.Models.VisaModels;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System.Net;

namespace API_Getway.Handlers.PaymentHandlers
{
    public class VisaPaymentHandler : IPaymentPrivder
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GeneralSettings _generalSettings;
        private readonly IDbConnector _dbConnector;

        public VisaPaymentHandler(IHttpClientFactory httpClientFactory, GeneralSettings generalSettings, IDbConnector dbConnector)
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
                        .HandleResult<HttpResponseMessage>(r =>
                        {
                            if (r.StatusCode == HttpStatusCode.OK)
                            {
                                var jsonString = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                var result = JsonConvert.DeserializeObject<VisaSuccessResponse>(jsonString);
                                if (result?.ChargeResult.ToLower() == "failure")
                                {
                                    return true;
                                }
                            }
                            return !r.IsSuccessStatusCode;
                        })
                        .Or<HttpRequestException>()
                        .WaitAndRetry(_generalSettings.RetryAttempt, retryAttempt =>
                                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            HttpResponseMessage httpResponseMessage = httpRetryPolicy.Execute(
                    () =>
                    {
                        VisaPaymentModal visaPaymentModal = new(paymentModal);
                        string json = JsonConvert.SerializeObject(visaPaymentModal);
                        StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        httpClient.DefaultRequestHeaders.Add("identifier", "Avia");
                        var response = httpClient.PostAsync(_generalSettings.VisaEndpoint, httpContent).GetAwaiter().GetResult();
                        var jsonString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = JsonConvert.DeserializeObject<VisaSuccessResponse>(jsonString);
                            if (result?.ChargeResult.ToLower() == "failure")
                            {
                                _dbConnector.AddDeclineReasonToDb(merchantId, result.ResultReason);
                                isBuisnessError = true;
                            }
                            else if (result?.ChargeResult.ToLower() == "success")
                            {
                                isChargeSuccess = true;
                            }
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
