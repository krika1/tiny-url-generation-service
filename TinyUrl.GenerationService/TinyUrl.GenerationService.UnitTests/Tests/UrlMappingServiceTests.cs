using Moq;
using TinyUrl.GenerationService.Bussiness.Services;
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
        private readonly UrlMappingService _urlMappingService;

        public UrlMappingServiceTests()
        {
            _mockUrlMappingRepository = new Mock<IUrlMappingRepository>();
            _urlMappingService = new UrlMappingService(_mockUrlMappingRepository.Object);
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

        [Fact]
        public async Task DeleteShortUrlAsync_ValidShortUrlAndUserId_ShouldDeleteUrl()
        {
            // Arrange
            string shortUrl = "abc123";
            int userId = 1;
            var urlMapping = new UrlMapping { ShortUrl = shortUrl, UserId = userId };

            _mockUrlMappingRepository
                .Setup(repo => repo.GetUrlMappingAsync(shortUrl))
                .ReturnsAsync(urlMapping);

            // Act
            await _urlMappingService.DeleteShortUrlAsync(shortUrl, userId);

            // Assert
            _mockUrlMappingRepository.Verify(repo => repo.DeleteUrlMapping(shortUrl), Times.Once);
        }

        [Fact]
        public async Task DeleteShortUrlAsync_ShortUrlNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            string shortUrl = "abc123";
            int userId = 1;

            _mockUrlMappingRepository
                .Setup(repo => repo.GetUrlMappingAsync(shortUrl))
                .ReturnsAsync((UrlMapping)null); // No URL mapping found

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _urlMappingService.DeleteShortUrlAsync(shortUrl, userId));

            _mockUrlMappingRepository.Verify(repo => repo.DeleteUrlMapping(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteShortUrlAsync_UnauthorizedUser_ShouldThrowNotFoundException()
        {
            // Arrange
            string shortUrl = "abc123";
            int userId = 1;
            var urlMapping = new UrlMapping { ShortUrl = shortUrl, UserId = 2 }; // Different userId

            _mockUrlMappingRepository
                .Setup(repo => repo.GetUrlMappingAsync(shortUrl))
                .ReturnsAsync(urlMapping);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _urlMappingService.DeleteShortUrlAsync(shortUrl, userId));

            _mockUrlMappingRepository.Verify(repo => repo.DeleteUrlMapping(It.IsAny<string>()), Times.Never);
        }
    }
}
