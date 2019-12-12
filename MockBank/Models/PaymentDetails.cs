using System;

namespace MockBank.Models
{
    public class PaymentDetails
    {
        public long CardNumber { get; set; }
        public int CCV { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        
        //Auto-generated
        public override bool Equals(object obj)
        {
            var details = obj as PaymentDetails;
            return details != null &&
                   CardNumber == details.CardNumber &&
                   CCV == details.CCV &&
                   Amount == details.Amount &&
                   Timestamp == details.Timestamp;
        }

        //Auto-generated
        public override int GetHashCode()
        {
            var hashCode = 189736765;
            hashCode = hashCode * -1521134295 + CardNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + CCV.GetHashCode();
            hashCode = hashCode * -1521134295 + Amount.GetHashCode();
            hashCode = hashCode * -1521134295 + Timestamp.GetHashCode();
            return hashCode;
        }
    }
}