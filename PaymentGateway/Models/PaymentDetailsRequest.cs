using Microsoft.AspNetCore.Mvc;
using System;

namespace PaymentGateway.Models
{
    public class PaymentDetailsRequest
    {
        public string SupplierId { get; set; }
        public PaymentDetails Payment { get; set; }
    }
}