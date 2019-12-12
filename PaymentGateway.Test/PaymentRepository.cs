using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Test
{
    public class PaymentRepository
    {
        [Fact]
        public void GetShouldReturnElement()
        {
            var merchantId = 1;
            var supplierId = "1";
            var transactionId = 1;
            var expectedTransaction = new TransactionPayment()
            {
                TransactionId = transactionId,
                Payment = new TransactionPaymentDetails()
                {
                    SupplierId = supplierId
                },
            };

            var sut = new Services.PaymentRepository();

            sut.Insert(expectedTransaction, merchantId).Wait();

            var actual = sut.Get(merchantId, transactionId).Result;

            Assert.NotNull(actual);
            Assert.Equal(expectedTransaction.TransactionId, actual.TransactionId);
            var payment = actual.Payment;
            Assert.NotNull(payment);
            Assert.Equal(supplierId, payment.SupplierId);
        }

        [Fact]
        public void GetShouldExpectedTransaction()
        {
            var tasks = new List<Task>();
            var merchantId = 1;
            var supplierId = "1";
            var transactionsNr = 10;

            var sut = new Services.PaymentRepository();

            for (var i = 0; i < transactionsNr; i++)
            {
                var expectedTransaction = new TransactionPayment()
                {
                    TransactionId = i,
                    Payment = new TransactionPaymentDetails()
                    {
                        SupplierId = supplierId
                    }
                };

                tasks.Add(sut.Insert(expectedTransaction, merchantId));
            }

            Task.WaitAll(tasks.ToArray());

            var transactions = sut.Get(merchantId, 4).Result;

            Assert.NotNull(transactions);
        }

        [Fact]
        public void GetAllShouldObtainSeveralTransactions()
        {
            var tasks = new List<Task>();
            var merchantId = 1;
            var supplierId = "1";
            var transactionsNr = 10;

            var sut = new Services.PaymentRepository();

            for (var i = 0; i < transactionsNr; i++)
            {
                var expectedTransaction = new TransactionPayment()
                {
                    TransactionId = i,
                    Payment = new TransactionPaymentDetails()
                    {
                        SupplierId = supplierId
                    }
                };

                tasks.Add(sut.Insert(expectedTransaction, merchantId));
            }

            Task.WaitAll(tasks.ToArray());

            var transactions = sut.GetAllByMerchantId(merchantId).Result;

            Assert.NotNull(transactions);
            Assert.Equal(transactionsNr, transactions.Count());
        }

        [Fact]
        public void GetAllShouldReturnTransactionsFromExpectedMerchant()
        {
            var tasks = new List<Task>();
            var transactionsNr = 10;

            var sut = new Services.PaymentRepository();

            for (var i = 0; i < transactionsNr; i++)
            {
                var expectedTransaction = new TransactionPayment()
                {
                    TransactionId = i,
                    Payment = new TransactionPaymentDetails()
                };

                tasks.Add(sut.Insert(expectedTransaction, (i % 2) + 1));
            }

            Task.WaitAll(tasks.ToArray());

            var transactions = sut.GetAllByMerchantId(1).Result;

            Assert.NotNull(transactions);
            Assert.Equal(transactionsNr / 2, transactions.Count());
            foreach(var transaction in transactions)
            {
                Assert.NotNull(transaction.Payment);
            }
        }

        [Fact]
        public void UpdateStatusShouldOnlyChangeStatus()
        {
            var merchantId = 1;
            var supplierId = "1";
            var transactionId = 1;
            var expectedTransaction = new TransactionPayment()
            {
                TransactionId = transactionId,
                Status = PaymentStatus.Failed,
                Payment = new TransactionPaymentDetails()
                {
                    SupplierId = supplierId,
                    Amount = 1,
                    CardNumber = 1,
                    CCV = 1,
                    Timestamp = DateTime.MaxValue
                }
            };

            var sut = new Services.PaymentRepository();

            sut.Insert(expectedTransaction, merchantId).Wait();

            var updateTransaction = new TransactionPayment()
            {
                TransactionId = transactionId,
                Status = PaymentStatus.Success,
                Payment = new TransactionPaymentDetails()
                {
                    SupplierId = supplierId,
                    Amount = 2,
                    CardNumber = 2,
                    CCV = 2,
                    Timestamp = DateTime.MaxValue
                }
            };

            var actual = sut.UpdateTransactionStatus(updateTransaction, merchantId).Result;

            Assert.NotNull(actual);
            Assert.Equal(expectedTransaction, actual);
        }

        [Fact]
        public void UpdateStatusShouldThrowExceptionIfMerchantDoesNotExist()
        {
            var updateTransaction = new TransactionPayment()
            {
                TransactionId = 1,
                Payment = new TransactionPaymentDetails()
            };

            var sut = new Services.PaymentRepository();

            Assert.Throws<ArgumentException>(() => sut.UpdateTransactionStatus(updateTransaction,1).Result);
        }

        [Fact]
        public void UpdateStatusShouldThrowExceptionIfTransactionDoesNotExist()
        {
            var merchantId = 1;
            var transactionId = 1;
            var expectedTransaction = new TransactionPayment()
            {
                TransactionId = transactionId,
                Payment = new TransactionPaymentDetails()
            };

            var updateTransaction = new TransactionPayment()
            {
                TransactionId = 2,
                Payment = new TransactionPaymentDetails()
            };

            var sut = new Services.PaymentRepository();

            sut.Insert(expectedTransaction, merchantId).Wait();

            Assert.Throws<ArgumentException>(() => sut.UpdateTransactionStatus(updateTransaction, merchantId).Result);
        }
    }
}
