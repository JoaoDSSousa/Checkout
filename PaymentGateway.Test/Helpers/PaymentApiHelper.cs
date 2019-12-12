using Moq;
using Moq.Protected;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Test.Helpers
{
    public static class PaymentApiHelper
    {
        public static Mock<HttpMessageHandler> CreateMockMessageHandler(HttpResponseMessage response)
        {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response)
            .Verifiable();

            return mockHandler;
        }

        public static void VerifyMockIsCalledWithMethod(this Mock<HttpMessageHandler> mockHandler, HttpMethod method)
        {
            mockHandler
            .Protected()
            .Verify("SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == method
                ),
                ItExpr.IsAny<CancellationToken>());
        }

        public static void VerifyMockIsCalledWithRightRelativePath(this Mock<HttpMessageHandler> mockHandler, string expectedRelativeUrl)
        {
            mockHandler
            .Protected()
            .Verify("SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri.AbsolutePath.Contains(expectedRelativeUrl)),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}
