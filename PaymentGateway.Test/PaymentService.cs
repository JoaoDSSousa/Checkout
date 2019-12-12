using Moq;
using PaymentGateway.External;
using PaymentGateway.Models;
using PaymentGateway.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Test
{
    public class PaymentService
    {
        [Fact]
        public void ProcessPaymentShouldCallApiAndStoreResult()
        {
            var requestPayment = new MerchantPaymentDetails()
            {
                Amount = 1,
                CardNumber = 1,
                CCV = 1,
                MerchantId = 1,
                Timestamp = DateTime.MaxValue
            };

            var expectedTransaction = new TransactionPayment()
            {
                Status = PaymentStatus.Success,
                TransactionId = 1,
                Payment = new TransactionPaymentDetails()
                {
                    Amount = requestPayment.Amount,
                    CardNumber = requestPayment.CardNumber,
                    CCV = requestPayment.CCV,
                    Timestamp = requestPayment.Timestamp
                }
            };

            var mockApi = new Mock<IPaymentApi>();

            mockApi.Setup(a => a.ProcessPayment(It.IsAny<MerchantPaymentDetails>()))
                .Returns(Task.FromResult(expectedTransaction));

            var mockRepo = new Mock<IPaymentRepository>();

            mockRepo.Setup(a => a.Insert(
                    It.IsAny<TransactionPayment>(),
                    It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var sut = new Services.PaymentService(mockApi.Object, mockRepo.Object);
            
            var transaction = sut.ProcessPayment(requestPayment).Result;

            mockApi.Verify(a => a.ProcessPayment(
                It.Is<MerchantPaymentDetails>(p => 
                    requestPayment.Equals(p)
            )), Times.Once);

            mockRepo.Verify(a => a.Insert(
                It.Is<TransactionPayment>(t =>
                    expectedTransaction.Equals(t)),
                It.Is<int>(i => i == requestPayment.MerchantId)
            ), Times.Once);
        }

        [Fact]
        public void GetShouldCallRepositoryAndReturnTransaction()
        {
            var merchandId = 1;
            var transactionId = 1;

            var expectedPayment = new TransactionPaymentDetails()
            {
                Amount = 1,
                CardNumber = 1,
                CCV = 1,
                SupplierId = "1",
                Timestamp = DateTime.MaxValue
            };

            var expectedTransaction = new TransactionPayment()
            {
                Status = PaymentStatus.Success,
                TransactionId = transactionId,
                Payment = expectedPayment
            };

            var mockApi = new Mock<IPaymentApi>();
            var mockRepo = new Mock<IPaymentRepository>();

            mockRepo.Setup(a => a.Get(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(expectedTransaction));

            var sut = new Services.PaymentService(mockApi.Object, mockRepo.Object);

            var transaction = sut.Get(merchandId, transactionId).Result;

            Assert.NotNull(transaction);
            
            Assert.Equal(expectedTransaction.Status, transaction.Status);
            Assert.Equal(expectedTransaction.TransactionId, transaction.TransactionId);

            var payment = transaction.Payment;
            Assert.NotNull(payment);
            Assert.Equal(expectedPayment.Amount, payment.Amount);
            Assert.Equal(expectedPayment.CardNumber, payment.CardNumber);
            Assert.Equal(expectedPayment.CCV, payment.CCV);
            Assert.Equal(merchandId, payment.MerchantId);
            Assert.Equal(expectedPayment.Timestamp, payment.Timestamp);

            mockRepo.Verify(a => a.Get(
                It.Is<int>(m => m == merchandId),
                It.Is<int>(t => t == transactionId)
            ), Times.Once);
        }

        [Fact]
        public void GetAllByMerchantShouldCallRepositoryAndReturnAllMerchantsTransactions()
        {
            var merchandId = 1;
            var transactionNr = 10;
            var expectedTransactions = new List<TransactionPayment>();

            for (var i = 0; i < transactionNr; i++)
            {

                var expectedTransaction = new TransactionPayment()
                {
                    Status = PaymentStatus.Success,
                    TransactionId = i,
                    Payment = new TransactionPaymentDetails()
                    {
                        Amount = 1,
                        CardNumber = 1,
                        CCV = 1,
                        SupplierId = "1",
                        Timestamp = DateTime.MaxValue
                    }
                };
                expectedTransactions.Add(expectedTransaction);
            }

            var mockApi = new Mock<IPaymentApi>();
            var mockRepo = new Mock<IPaymentRepository>();

            mockRepo.Setup(a => a.GetAllByMerchantId(It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<TransactionPayment>>(expectedTransactions));

            var sut = new Services.PaymentService(mockApi.Object, mockRepo.Object);

            var transactions = sut.GetAllByMerchantId(merchandId).Result;

            Assert.NotNull(transactions);

            Assert.Equal(transactionNr, transactions.Count());

            mockRepo.Verify(a => a.GetAllByMerchantId(It.Is<int>(m => m == merchandId)), Times.Once);
        }
    }
}
