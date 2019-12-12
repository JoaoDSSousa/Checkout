using PaymentGateway.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDictionary<int, IEnumerable<TransactionPayment>> transactions;

        public PaymentRepository()
        {
            transactions = new ConcurrentDictionary<int, IEnumerable<TransactionPayment>>();
        }

        public Task<TransactionPayment> Get(int merchantId, int transactionId)
        {
            if (!transactions.ContainsKey(merchantId))
            {
                return Task.FromResult(default(TransactionPayment));
            }

            return Task.FromResult(
                transactions[merchantId]
                    .FirstOrDefault( t => t.TransactionId == transactionId));
        }

        public Task<IEnumerable<TransactionPayment>> GetAllByMerchantId(int merchantId)
        {
            if (!transactions.ContainsKey(merchantId))
            {
                return Task.FromResult(Enumerable.Empty<TransactionPayment>());
            }

            return Task.FromResult(transactions[merchantId]);
        }

        public Task Insert(TransactionPayment elem, int merchantId)
        {
            if (!transactions.ContainsKey(merchantId))
            {
                transactions.Add(merchantId, new ConcurrentBag<TransactionPayment>() { elem });
                return Task.CompletedTask;
            }

            var transaction = transactions[merchantId];

            transactions[merchantId] = Enumerable.Append(transaction, elem);

            return Task.CompletedTask;
        }

        public Task<TransactionPayment> UpdateTransactionStatus(TransactionPayment elem, int merchantId)
        {
            if (!transactions.ContainsKey(merchantId))
            {
                throw new ArgumentException($"Merchant id {merchantId} does not exist");
            }

            var transaction = transactions[merchantId];
            var payment = transaction.FirstOrDefault(t =>
                t.TransactionId == elem.TransactionId);

            if(payment == null)
            {
                throw new ArgumentException($"Merchant id {merchantId} does not have a transaction {elem.TransactionId}");
            }

            payment.Status = elem.Status;

            return Task.FromResult(payment);
        }
    }
}