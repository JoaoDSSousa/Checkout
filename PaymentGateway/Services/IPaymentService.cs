using PaymentGateway.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public interface IPaymentService
    {
        /// <summary>
        /// Processes a payment and stores it.
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        Task<MerchantTransactionPayment> ProcessPayment(MerchantPaymentDetails details);

        /// <summary>
        /// Obtain the details of a transaction for a given merchant.
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        Task<MerchantTransactionPayment> Get(int merchantId, int transactionId);

        /// <summary>
        /// Obtain all the transactions for a given merchant.
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Task<IEnumerable<MerchantTransactionPayment>> GetAllByMerchantId(int merchantId);
    }
}
