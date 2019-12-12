using System.Collections.Generic;

namespace PaymentGateway.Models
{
    public class MerchantTransactionPayment
    {
        public MerchantPaymentDetails Payment { get; set; }
        public PaymentStatus Status { get; set; }
        public int TransactionId { get; set; }
        
        public MerchantTransactionPayment()
        { }

        public MerchantTransactionPayment(TransactionPayment transaction)
        {
            Status = transaction.Status;
            TransactionId = transaction.TransactionId;
            Payment = new MerchantPaymentDetails(transaction.Payment);
        }
    }
}