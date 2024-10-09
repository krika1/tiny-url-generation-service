﻿using TinyUrl.GenerationService.Infrastructure.Context;
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

        public async Task<UrlMapping> CreateUrlMappingAsyc(UrlMapping urlMapping)
        {
            await _dbContext.UrlMappings.InsertOneAsync(urlMapping);

            return urlMapping;
        }
    }
}
