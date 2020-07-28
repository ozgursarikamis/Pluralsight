using System.Net;
using Moq;
using Movies.Client;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq.Protected;
using Xunit;

namespace Movies.Test
{
    public class TestableClassWithApiAccessUnitTests
    {
        [Fact]
        public void GetMovie_On401Response_MustThrowUnauthorizedApiAccessException()
        {
            var httpClient = new HttpClient(new Return401UnauthorizedResponseHandler());
            var testableClass = new TestableClassesWithApiAccess(httpClient);

            var cancellationTokenSource = new CancellationTokenSource();

            Assert.ThrowsAsync<UnauthorizedApiAccessException>(() =>
                testableClass.GetMovie(cancellationTokenSource.Token));
        }

        [Fact]
        public void GetMovie_On401Response_MustThrowUnauthorizedApiAccessException_WithMoq()
        {
            var unauthorizedResponse = new Mock<HttpMessageHandler>();
            unauthorizedResponse.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized
                });

            var httpClient = new HttpClient(unauthorizedResponse.Object);

            var testableClass = new TestableClassesWithApiAccess(httpClient);
            var cancellationTokenSource = new CancellationTokenSource();

            Assert.ThrowsAsync<UnauthorizedApiAccessException>(() =>
                testableClass.GetMovie(cancellationTokenSource.Token));
        }
    }
}
