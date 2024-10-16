﻿using TinyUrl.GenerationService.Infrastructure.Contracts.Requests;
using TinyUrl.GenerationService.Infrastructure.Contracts.Responses;

namespace TinyUrl.GenerationService.Infrastructure.Services
{
    public interface IUrlMappingService
    {
        Task<UrlMappingContract> ShortenUrlAsync(ShortenUrlRequest request, int userId);
        Task<IEnumerable<UrlMappingContract>> GetAllUrlMappingsAsync(int userId);
        Task DeleteShortUrlAsync(string shortUrl, int userId);
        Task SetExpirationDateAsync(UpdateDateExpirationRequest request, string shortUrl, int userId);
    }
}
