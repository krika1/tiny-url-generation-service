using Moq;
using TinyUrl.GenerationService.Bussiness.Services;
using TinyUrl.GenerationService.Infrastructure.Clients;
using TinyUrl.GenerationService.Infrastructure.Contracts.Requests;
using TinyUrl.GenerationService.Infrastructure.Contracts.Responses;
using TinyUrl.GenerationService.Infrastructure.Entities;
using TinyUrl.GenerationService.Infrastructure.Exceptions;
using TinyUrl.GenerationService.Infrastructure.Repositories;

namespace TinyUrl.GenerationService.UnitTests.Tests
{
    public class UrlMappingServiceTests
    {
        private readonly Mock<IUrlMappingRepository> _mockUrlMappingRepository;
        private readonly Mock<IUserServiceClient> _mockUserServiceClient;
        private readonly UrlMappingService _urlMappingService;

        public UrlMappingServiceTests()
        {
            _mockUrlMappingRepository = new Mock<IUrlMappingRepository>();
            _mockUserServiceClient = new Mock<IUserServiceClient>();
            _urlMappingService = new UrlMappingService(_mockUrlMappingRepository.Object, _mockUserServiceClient.Object);
        }

        [Fact]
        public async Task ShortenUrlAsync_ValidUser_ReturnsShortUrl()
        {
            // Arrange
            var request = new ShortenUrlRequest
            {
                LongUrl = "https://example.com/some-long-url"
            };

            var user = new UserContract();  // Simulate a valid user

            _mockUrlMappingRepository.Setup(repo => repo.CreateUrlMappingAsyc(It.IsAny<UrlMapping>()))
                .ReturnsAsync(new UrlMapping { ShortUrl = "https://short.url/abc123" });

            // Act
            var result = await _urlMappingService.ShortenUrlAsync(request, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("https://short.url/abc123", result.ShortUrl);
            _mockUrlMappingRepository.Verify(r => r.CreateUrlMappingAsyc(It.IsAny<UrlMapping>()), Times.Once);
        }
    }
}
