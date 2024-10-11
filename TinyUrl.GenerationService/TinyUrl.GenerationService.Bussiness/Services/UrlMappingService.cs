﻿using System.Security.Cryptography;
using System.Text;
using TinyUrl.GenerationService.Infrastructure.Clients;
using TinyUrl.GenerationService.Infrastructure.Common;
using TinyUrl.GenerationService.Infrastructure.Contracts.Requests;
using TinyUrl.GenerationService.Infrastructure.Contracts.Responses;
using TinyUrl.GenerationService.Infrastructure.Exceptions;
using TinyUrl.GenerationService.Infrastructure.Mappings;
using TinyUrl.GenerationService.Infrastructure.Repositories;
using TinyUrl.GenerationService.Infrastructure.Services;

namespace TinyUrl.GenerationService.Bussiness.Services
{
    public class UrlMappingService : IUrlMappingService
    {
        private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private readonly IUrlMappingRepository _urlMappingRepository;
        private readonly IUserServiceClient _userClient;

        public UrlMappingService(IUrlMappingRepository urlMappingRepository, IUserServiceClient userClient)
        {
            _urlMappingRepository = urlMappingRepository;
            _userClient = userClient;
        }

        public async Task<UrlMappingContract> ShortenUrlAsync(ShortenUrlRequest request, int userId)
        { 
            var shortUrl = GenerateShortUrl(request.LongUrl!);

            var urlMapping = UrlMappingMapping.ToDomain(request);
            urlMapping.ShortUrl = shortUrl;
            urlMapping.UserId = userId;

            var createdMapping = await _urlMappingRepository.CreateUrlMappingAsyc(urlMapping).ConfigureAwait(false);

            return UrlMappingMapping.ToContract(createdMapping);
        }

        private string GenerateShortUrl(string longUrl)
        {
            string result = string.Empty;

            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(longUrl);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                var md5String = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                var hex = md5String.Substring(0, 12);

                var hexToDecimal = Convert.ToInt64(hex, 16);

                while (hexToDecimal > 0)
                {
                    result = Base62Chars[(int)(hexToDecimal % 62)] + result;
                    hexToDecimal /= 62;
                }
            }

            return AttachBaseUrl(result);
        }

        private string AttachBaseUrl(string hashedUrl)
        {
            return $"https://localhost:7111/{hashedUrl}";
        }
    }
}
