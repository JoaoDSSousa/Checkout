namespace MockBank.Models
{
    public class TransactionPayment
    {
        public TransactionPaymentDetails Payment { get; set; }
        public PaymentStatus Status { get; set; }
        public int TransactionId { get; set; }
    }
}