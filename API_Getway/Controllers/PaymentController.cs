using API_Getway.Interfaces;
using API_Getway.Models;
using API_Getway.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API_Getway.Controllers
{
    [ApiController]
    [Route("api")]
    public class PaymentController : ControllerBase
    {
        private readonly IPayment _payment;
        private readonly IValidation _validation;

        public PaymentController(IPayment payment, IValidation validation)
        { 
            _payment = payment;
            _validation = validation;
        }

        [HttpPost]
        [Route("charge")]
        public IActionResult Charge([FromHeader(Name = "merchant-identifier")] string merchantId, CreateChargeDTO input)
        {
            _validation.ValidateChargeInput(merchantId, input);
            PaymentModal paymentModal = new(input);
            string result = _payment.Charge(merchantId, input.CreditCardCompany, paymentModal);
            if (!string.IsNullOrWhiteSpace(result))
            {
                return Ok(new CreateChargeResponseDTO { Error = result});
            }
            return Ok();
        }

        [HttpGet]
        [Route("chargeStatuses")]
        public IActionResult ChargeStatuses([FromHeader(Name = "merchant-identifier")] string merchantId)
        {
            if (string.IsNullOrWhiteSpace(merchantId))
            {
                throw new InvalidOperationException("invalid merchantId");
            }
            var result = _payment.ChargeStatuses(merchantId);
            var x = new ChargeStatusesDTOResponse(result);
            var res = JsonConvert.SerializeObject(x.Reasons);
            return Ok(res);
        }
    }
}