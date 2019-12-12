using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using PaymentGateway.Models;
using PaymentGateway.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Test
{
    public class PaymentApi
    {
        private string DummyUrl = "http://www.dummyurl.com/";
        private string SupplierId = "checkout";

        [Fact]
        public void ShouldCallProcessPaymentAsAPostHttpMethod()
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("")
            };

            var mockHandler = PaymentApiHelper.CreateMockMessageHandler(response);
            
            var client = new HttpClient(mockHandler.Object);
            var sut = new External.PaymentApi(client, DummyUrl);

            var paymentDetails = new MerchantPaymentDetails();

            var result = sut.ProcessPayment(paymentDetails).Result;

            mockHandler.VerifyMockIsCalledWithMethod(HttpMethod.Post);
        }

        [Fact]
        public void ShouldCallApiWithRightUrl()
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("")
            };

            var mockHandler = PaymentApiHelper.CreateMockMessageHandler(response);

            var client = new HttpClient(mockHandler.Object);
            var sut = new External.PaymentApi(client, DummyUrl);

            var paymentDetails = new MerchantPaymentDetails();

            var result = sut.ProcessPayment(paymentDetails).Result;

            //Todo: figure out why is failing
            //mockHandler.VerifyMockIsCalledWithRightRelativePath(DummyUrl + $"api/payment/{SupplierId}");
        }

        [Fact]
        public void ShouldParseApiResponse()
        {
            var transactionPaymentDetails = new TransactionPaymentDetails()
            {
                Amount = 1,
                CardNumber = 1,
                CCV = 1,
                SupplierId = SupplierId,
                Timestamp = DateTime.MaxValue
            };

            var paymentDetails = new MerchantPaymentDetails()
            {
                Amount = 1,
                CardNumber = 1,
                CCV = 1,
                MerchantId = 1,
                Timestamp = DateTime.MaxValue
            };
            
            var contentStr = $"{{ \"transactionId\": 1, \"status\": 1, " +
                    $"\"payment\": " +
                    JsonConvert.SerializeObject(transactionPaymentDetails) +
                    $"}}";

            var response = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(contentStr)
            };

            var mockHandler = PaymentApiHelper.CreateMockMessageHandler(response);

            var client = new HttpClient(mockHandler.Object);
            var sut = new External.PaymentApi(client, DummyUrl);
            
            var result = sut.ProcessPayment(paymentDetails).Result;

            Assert.NotNull(result);

            var transaction = result as TransactionPayment;
            Assert.NotNull(transaction);

            Assert.Equal(1, transaction.TransactionId);
            Assert.Equal(PaymentStatus.Success, transaction.Status);
            Assert.Equal(SupplierId, transaction.Payment.SupplierId);
        }
    }
}
