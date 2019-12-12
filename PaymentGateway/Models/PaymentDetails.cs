using System;

namespace PaymentGateway.Models
{
    public class PaymentDetails
    {
        public long CardNumber { get; set; }
        public int CCV { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        
        public PaymentDetails()
        { }

        public PaymentDetails(MerchantPaymentDetails payment)
        {
            CardNumber = payment.CardNumber;
            CCV = payment.CCV;
            Amount = payment.Amount;
            Timestamp = payment.Timestamp;
        }
    }
}