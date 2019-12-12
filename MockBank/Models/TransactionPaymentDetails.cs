using System;
using System.Collections.Generic;

namespace MockBank.Models
{
    public class TransactionPaymentDetails
    {
        public long CardNumber { get; set; }
        public int CCV { get; set; }
        public decimal Amount { get; set; }
        public string SupplierId { get; set; }
        public DateTime Timestamp { get; set; }

        public TransactionPaymentDetails()
        { }

        public TransactionPaymentDetails(PaymentDetailsRequest payment)
        {
            SupplierId = payment.SupplierId;

            var paymentDetails = payment.Payment;
            CardNumber = paymentDetails.CardNumber;
            CCV = paymentDetails.CCV;
            Amount = paymentDetails.Amount;
            Timestamp = paymentDetails.Timestamp;
        }

        //Auto-generated
        public override bool Equals(object obj)
        {
            var details = obj as TransactionPaymentDetails;
            return details != null &&
                   CardNumber == details.CardNumber &&
                   CCV == details.CCV &&
                   Amount == details.Amount &&
                   SupplierId == details.SupplierId &&
                   Timestamp == details.Timestamp;
        }

        //Auto-generated
        public override int GetHashCode()
        {
            var hashCode = 189736765;
            hashCode = hashCode * -1521134295 + CardNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + CCV.GetHashCode();
            hashCode = hashCode * -1521134295 + Amount.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SupplierId);
            hashCode = hashCode * -1521134295 + Timestamp.GetHashCode();
            return hashCode;
        }
    }
}