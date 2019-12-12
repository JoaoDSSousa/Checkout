using System;

namespace PaymentGateway.Models
{
    public class MerchantPaymentDetails
    {
        public long CardNumber { get; set; }
        public int CCV { get; set; }
        public decimal Amount { get; set; }
        public int MerchantId { get; set; }
        public DateTime Timestamp { get; set; }

        public MerchantPaymentDetails()
        { }
        
        public MerchantPaymentDetails(TransactionPaymentDetails payment)
        {
            CardNumber = payment.CardNumber;
            CCV = payment.CCV;
            Amount = payment.Amount;
            Timestamp = payment.Timestamp;
            CardNumber = payment.CardNumber;
        }

        public MerchantPaymentDetails(MerchantPaymentDetailsRequest merchantPayment)
        {
            MerchantId = merchantPayment.MerchantId;

            var payment = merchantPayment.Payment;
            CardNumber = payment.CardNumber;
            CCV = payment.CCV;
            Amount = payment.Amount;
            Timestamp = payment.Timestamp;
            CardNumber = payment.CardNumber;
        }
    }
}