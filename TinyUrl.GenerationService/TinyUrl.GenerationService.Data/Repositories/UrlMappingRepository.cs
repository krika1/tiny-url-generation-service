using MongoDB.Driver;
using TinyUrl.GenerationService.Infrastructure.Context;
using TinyUrl.GenerationService.Infrastructure.Entities;
using TinyUrl.GenerationService.Infrastructure.Repositories;

namespace TinyUrl.GenerationService.Data.Repositories
{
    public class UrlMappingRepository : IUrlMappingRepository
    {
        private readonly MongoDbContext _dbContext;

        public UrlMappingRepository(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UrlMapping> GetUrlMappingAsync(string shortUrl)
        {
            var mapping = await _dbContext.UrlMappings.Find(um => um.ShortUrl!.Contains(shortUrl)).FirstOrDefaultAsync();

            return mapping!;
        }

        public async Task<UrlMapping> CreateUrlMappingAsyc(UrlMapping urlMapping)
        {
            await _dbContext.UrlMappings.InsertOneAsync(urlMapping);

            return urlMapping;
        }

        public async Task DeleteUrlMapping(string shortUrl)
        {
            await _dbContext.UrlMappings.DeleteOneAsync(um => um.ShortUrl!.Contains(shortUrl));
        }

        public async Task<IEnumerable<UrlMapping>> GetAllUrlMappings(int userId)
        {
            var urls = await _dbContext.UrlMappings.Find(um => um.UserId == userId).ToListAsync();

            return urls;
        }

        public async Task UpdateUrlMapping(UrlMapping urlMapping)
        {
           var filter = Builders<UrlMapping>.Filter.Eq(e => e.Id, urlMapping.Id);

           var update = Builders<UrlMapping>.Update
               .Set(e => e.ExpirationDate, urlMapping.ExpirationDate);

           await _dbContext.UrlMappings.UpdateOneAsync(filter, update);
        }
    }
}
