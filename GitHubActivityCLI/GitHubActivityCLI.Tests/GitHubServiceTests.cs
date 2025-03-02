using NUnit.Framework;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using Moq.Protected;

namespace GitHubActivityCLI.Tests
{
    [TestFixture]
    public class GitHubServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private GitHubService _gitHubService;

        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new System.Uri("https://api.github.com/")
            };

            _gitHubService = new GitHubService(_httpClient);
        }

        [Test]
        public async Task FetchGitHubActivity_ShouldReturnFormattedActivity_WhenApiResponseIsValid()
        {
            // Arrange: Simulated API response
            string jsonResponse = "[{ \"type\": \"PushEvent\", \"repo\": { \"name\": \"user/repo1\" } }]";
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            // Act
            await _gitHubService.FetchGitHubActivity("testuser");

            // Assert: Ensure API is called exactly once with GET
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public async Task FetchGitHubActivity_ShouldHandleError_WhenApiFails()
        {
            // Arrange: Simulated API failure
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            // Act
            await _gitHubService.FetchGitHubActivity("testuser");

            // Assert: Ensure API is called once even if it fails
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            );
        }

    }
}