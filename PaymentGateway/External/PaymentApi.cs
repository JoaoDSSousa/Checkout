using Newtonsoft.Json;
using PaymentGateway.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PaymentGateway.External
{
    public class PaymentApi : IPaymentApi
    {
        private readonly HttpClient client;
        private readonly string baseUrl;
        private static readonly string SupplierId = "checkout";

        public PaymentApi(HttpClient client, string baseUrl)
        {
            this.client = client;
            this.baseUrl = baseUrl;
        }

        public async Task<TransactionPayment> ProcessPayment(MerchantPaymentDetails details)
        {
            var httpRequest = new HttpRequestMessage()
            {
                RequestUri = new Uri(new Uri(baseUrl), $"api/payment/{SupplierId}")
            };
            
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpRequest.Method = HttpMethod.Post;

            var content = new PaymentDetails(details);

            var contentStr = JsonConvert.SerializeObject(content);

            httpRequest.Content = new StringContent(
                contentStr,
                System.Text.Encoding.UTF8,
                "application/json");

            var httpResponse = await client.SendAsync(httpRequest);
            var response = JsonConvert.DeserializeObject<TransactionPayment>(
                await httpResponse.Content.ReadAsStringAsync());

            return response;
        }
    }
}
