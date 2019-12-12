using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MockBank.Models;

namespace MockBank.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        // POST api/Payment
        [HttpPost("{SupplierId}")]
        public TransactionPayment Post(PaymentDetailsRequest payment)
        {
            var transactionDetails = new TransactionPaymentDetails(payment);
            return new TransactionPayment
            {
                TransactionId = transactionDetails.GetHashCode(),
                Payment = transactionDetails,
                Status = PaymentStatus.Success
            };
        }
        
    }
}
