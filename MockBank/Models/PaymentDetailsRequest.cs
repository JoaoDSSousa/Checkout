using Microsoft.AspNetCore.Mvc;
using System;

namespace MockBank.Models
{
    public class PaymentDetailsRequest
    {
        [FromRoute]
        public string SupplierId { get; set; }
        
        [FromBody]
        public PaymentDetails Payment { get; set; }
    }
}