using PaymentGateway.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public interface IPaymentRepository
    {
        /// <summary>
        /// Inserts payment transaction
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Task Insert(TransactionPayment elem, int merchantId);

        /// <summary>
        /// Updates transaction status. 
        /// Throws ArgumentException if Merchant Id or Transaction Id do not exist
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Task<TransactionPayment> UpdateTransactionStatus(TransactionPayment elem, int merchantId);

        /// <summary>
        /// Get a payment transaction by Id and Merchant Id
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        Task<TransactionPayment> Get(int merchantId, int transactionId);

        /// <summary>
        /// Get all the payment transactions for a given merchant.
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Task<IEnumerable<TransactionPayment>> GetAllByMerchantId(int merchantId);
    }
}