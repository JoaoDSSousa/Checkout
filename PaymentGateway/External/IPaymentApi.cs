using PaymentGateway.Models;
using System.Threading.Tasks;

namespace PaymentGateway.External
{
    public interface IPaymentApi
    {
        Task<TransactionPayment> ProcessPayment(MerchantPaymentDetails details);
    }
}
