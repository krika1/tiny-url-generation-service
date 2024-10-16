﻿using TinyUrl.GenerationService.Infrastructure.Contracts.Requests;
using TinyUrl.GenerationService.Infrastructure.Contracts.Responses;
using TinyUrl.GenerationService.Infrastructure.Entities;

namespace TinyUrl.GenerationService.Infrastructure.Mappings
{
    public static class UrlMappingMapping
    {
        public static UrlMapping ToDomain(ShortenUrlRequest request)
        {
            return new UrlMapping { LongUrl = request.LongUrl, Clicks = 0, CreationDate = DateTime.Now, ExpirationDate = request.ExpiryDate };
        }

        public static UrlMappingContract ToContract(UrlMapping urlMapping)
        {
            return new UrlMappingContract { LongUrl = urlMapping.LongUrl, ExpiryDate = urlMapping.ExpirationDate, CreatedAt = urlMapping.CreationDate, ShortUrl = urlMapping.ShortUrl, Clicks = urlMapping.Clicks };
        }
    }
}
