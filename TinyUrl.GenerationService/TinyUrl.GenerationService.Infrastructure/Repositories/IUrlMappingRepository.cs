using TinyUrl.GenerationService.Infrastructure.Entities;

namespace TinyUrl.GenerationService.Infrastructure.Repositories
{
    public interface IUrlMappingRepository
    {
        Task<UrlMapping> CreateUrlMappingAsyc(UrlMapping urlMapping);
        Task<UrlMapping> GetUrlMappingAsync(string shortUrl);
        Task DeleteUrlMapping(string shortUrl);
    }
}
