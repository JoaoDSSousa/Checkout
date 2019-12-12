using System;

namespace PaymentGateway.Models
{
    public class TransactionPaymentDetails
    {
        public long CardNumber { get; set; }
        public int CCV { get; set; }
        public decimal Amount { get; set; }
        public string SupplierId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}