using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Models;
using PaymentGateway.Services;

namespace PaymentGateway.Controllers
{
    [Route("api")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        
        // GET api/merchant/5/payment/1
        [HttpGet("merchant/{merchantId}/[controller]/{transactionId}")]
        public async Task<MerchantTransactionPayment> Get([FromRoute] int merchantId, [FromRoute] int transactionId)
        {
            var payment = await paymentService.Get(merchantId, transactionId);

            return payment;
        }

        // GET api/merchant/5/payment
        [HttpGet("merchant/{merchantId}/[controller]")]
        public async Task<IEnumerable<MerchantTransactionPayment>> GetAll([FromRoute] int merchantId)
        {
            var payments = await paymentService.GetAllByMerchantId(merchantId);
            
            return payments;
        }

        // POST api/merchant/5/payment
        [HttpPost("merchant/{MerchantId}/[controller]")]
        public async Task<MerchantTransactionPayment> Post(MerchantPaymentDetailsRequest payment)
        {
            if(payment == null || payment.MerchantId == 0 || ValidatePaymentDetails(payment.Payment))
            {
                throw new ArgumentException("Invalid data.");
            }

            var data = new MerchantPaymentDetails(payment);

            var processedPayment = await paymentService.ProcessPayment(data);
            
            return processedPayment;
        }

        private bool ValidatePaymentDetails(PaymentDetails payment) {
            return payment.Amount <= 0 
                || payment.CardNumber <= 0 
                || payment.CCV <= 0;
        }
    }
}
