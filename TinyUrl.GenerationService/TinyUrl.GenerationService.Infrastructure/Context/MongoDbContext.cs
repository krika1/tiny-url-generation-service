using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TinyUrl.GenerationService.Infrastructure.Contracts.Options;
using TinyUrl.GenerationService.Infrastructure.Entities;

namespace TinyUrl.GenerationService.Infrastructure.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _mongoDatabase;
        private const string URL_MAPPING_COLLECTION = "UrlsMapping";
        private readonly MongoDbOptions _options;

        public MongoDbContext(IMongoClient mongoClient, IOptions<MongoDbOptions> options)
        {
            _options = options.Value;
            _mongoDatabase = mongoClient.GetDatabase(_options.DatabaseName);
        }

        public IMongoCollection<UrlMapping> UrlMappings => _mongoDatabase.GetCollection<UrlMapping>(URL_MAPPING_COLLECTION);
    }
}
