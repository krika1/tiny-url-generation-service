using TinyUrl.GenerationService.Infrastructure.Contracts.Requests;
using TinyUrl.GenerationService.Infrastructure.Contracts.Responses;

namespace TinyUrl.GenerationService.Infrastructure.Services
{
    public interface IUrlMappingService
    {
        Task<UrlMappingContract> ShortenUrlAsync(ShortenUrlRequest request, int userId);
    }
}
