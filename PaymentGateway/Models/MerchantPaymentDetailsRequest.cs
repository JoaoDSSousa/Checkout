using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    public class MerchantPaymentDetailsRequest
    {
        [FromRoute]
        public int MerchantId { get; set; }

        [FromBody]
        public PaymentDetails Payment { get; set; }
    }
}