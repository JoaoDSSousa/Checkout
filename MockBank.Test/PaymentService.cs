using MockBank.Controllers;
using MockBank.Models;
using System;
using Xunit;

namespace MockBank.Test
{
    public class PaymentService
    {
        [Fact]
        public void ShouldReturnSameTransactionIdForSameDetails()
        {
            var sut = new PaymentController();

            var details1 = new PaymentDetails()
            {
                Amount = 1,
                CardNumber = 1234123412341234,
                CCV = 123,
                Timestamp = DateTime.MaxValue
            };

            var transaction1 = new PaymentDetailsRequest()
            {
                SupplierId = "123456",
                Payment = details1
            };
            
            var result1 = sut.Post(transaction1);

            var details2 = new PaymentDetails()
            {
                Amount = 1,
                CardNumber = 1234123412341234,
                CCV = 123,
                Timestamp = DateTime.MaxValue
            };

            var transaction2 = new PaymentDetailsRequest()
            {
                SupplierId = "123456",
                Payment = details2
            };

            var result2 = sut.Post(transaction2);

            Assert.Equal(result1.TransactionId, result2.TransactionId);
        }
    }
}
