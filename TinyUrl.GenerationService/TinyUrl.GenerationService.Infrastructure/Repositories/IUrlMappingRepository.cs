using TinyUrl.GenerationService.Infrastructure.Entities;

namespace TinyUrl.GenerationService.Infrastructure.Repositories
{
    public interface IUrlMappingRepository
    {
        Task<UrlMapping> CreateUrlMappingAsyc(UrlMapping urlMapping);
        Task<UrlMapping> GetUrlMappingAsync(string shortUrl);
        Task<IEnumerable<UrlMapping>> GetAllUrlMappings(int userId);
        Task DeleteUrlMapping(string shortUrl);
        Task UpdateUrlMapping(UrlMapping urlMapping);
    }
}
