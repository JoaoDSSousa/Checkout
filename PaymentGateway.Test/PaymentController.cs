using Moq;
using PaymentGateway.Models;
using PaymentGateway.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Test
{
    public class PaymentController
    {
        [Fact]
        public void PostShouldCallPaymentServiceProcessPayment()
        {
            var paymentRequest = new MerchantPaymentDetailsRequest()
            {
                MerchantId = 123456,
                Payment = new PaymentDetails()
                {
                    Amount = 1,
                    CardNumber = 1234123412341234,
                    CCV = 123,
                    Timestamp = DateTime.MaxValue
                }   
            };

            var payment = new MerchantPaymentDetails(paymentRequest);

            var transaction = new MerchantTransactionPayment()
            {
                Payment = payment,
                Status = PaymentStatus.Success,
                TransactionId = 1
            };

            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(ps => ps.ProcessPayment(It.IsAny<MerchantPaymentDetails>()))
                .Returns(Task.FromResult(transaction));

            var sut = new Controllers.PaymentController(paymentService.Object);

            var result = sut.Post(paymentRequest).Result;

            Assert.NotNull(result);
            paymentService.Verify(ps => ps.ProcessPayment(
                It.Is<MerchantPaymentDetails>(pd =>
                   payment.CardNumber == pd.CardNumber &&
                   payment.CCV == pd.CCV &&
                   payment.Amount == pd.Amount &&
                   payment.MerchantId == pd.MerchantId &&
                   payment.Timestamp == pd.Timestamp
                )), Times.Once);
        }

        [Theory]
        [InlineData(0, 1234123412341234,123,123456)]
        [InlineData(1, 0, 123, 123456)]
        [InlineData(1, 1234123412341234, 0, 123456)]
        [InlineData(1, 1234123412341234, 123, 0)]
        public void PostShouldValidateParametersAndThrowException(decimal amount, long cardNumber, int ccv, int merchantId)
        {
            var paymentRequest = new MerchantPaymentDetailsRequest()
            {
                MerchantId = merchantId,
                Payment = new PaymentDetails()
                {
                    Amount = amount,
                    CardNumber = cardNumber,
                    CCV = ccv,
                    Timestamp = DateTime.MaxValue
                }
            };

            var payment = new MerchantPaymentDetails(paymentRequest);

            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(ps => ps.ProcessPayment(It.IsAny<MerchantPaymentDetails>()))
                .Returns(Task.FromResult<MerchantTransactionPayment>(null));

            var sut = new Controllers.PaymentController(paymentService.Object);

            Assert.Throws<AggregateException>(() =>
            {
                var result = sut.Post(paymentRequest).Result;
            });

            paymentService.Verify(ps => ps.ProcessPayment(
                It.IsAny<MerchantPaymentDetails>()), Times.Never);
        }

        [Fact]
        public void GetAllShouldCallPaymentServiceGetAll()
        {
            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(ps => ps.GetAllByMerchantId(It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<MerchantTransactionPayment>>(null));

            var merchantId = 1;

            var sut = new Controllers.PaymentController(paymentService.Object);

            var payments = sut.GetAll(merchantId).Result;

            paymentService.Verify(ps => ps.GetAllByMerchantId(
                It.Is<int>(i => i == merchantId)), Times.Once);
        }

        [Fact]
        public void GetAllShouldCallPaymentServiceGet()
        {
            var paymentService = new Mock<IPaymentService>();
            paymentService.Setup(ps => ps.Get(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult<MerchantTransactionPayment>(null));

            var merchantId = 1;
            var transactionId = 1;

            var sut = new Controllers.PaymentController(paymentService.Object);

            var payments = sut.Get(merchantId, transactionId).Result;

            paymentService.Verify(ps => ps.Get(
                It.Is<int>(i => i == merchantId),
                It.Is<int>(i => i == transactionId)),
            Times.Once);
        }
    }
}
