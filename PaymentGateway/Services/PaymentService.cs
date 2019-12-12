using PaymentGateway.External;
using PaymentGateway.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentApi paymentApi;
        private readonly Services.IPaymentRepository repository;

        public PaymentService(IPaymentApi paymentApi, IPaymentRepository repository)
        {
            this.paymentApi = paymentApi;
            this.repository = repository;
        }

        public async Task<MerchantTransactionPayment> Get(int merchantId, int transactionId)
        {
            var transaction = await repository.Get(merchantId, transactionId);

            if(transaction == null)
            {
                return null;
            }

            var merchantTransaction = new MerchantTransactionPayment(transaction);
            merchantTransaction.Payment.MerchantId = merchantId;

            return merchantTransaction;
        }

        public async Task<IEnumerable<MerchantTransactionPayment>> GetAllByMerchantId(int merchantId)
        {
            var transactions = await repository.GetAllByMerchantId(merchantId);

            var merchantTransactions = transactions.Where(t => t != null)
                .Select(t =>
                {
                    var merchantTransaction = new MerchantTransactionPayment(t);
                    merchantTransaction.Payment.MerchantId = merchantId;
                    return merchantTransaction;
                });

            return merchantTransactions;
        }

        public async Task<MerchantTransactionPayment> ProcessPayment(MerchantPaymentDetails details)
        {
            var transaction = await paymentApi.ProcessPayment(details);

            var merchantId = details.MerchantId;

            await repository.Insert(transaction, merchantId);

            var merchantTransaction = new MerchantTransactionPayment(transaction);
            merchantTransaction.Payment.MerchantId = details.MerchantId;

            return merchantTransaction;
        }
    }
}
