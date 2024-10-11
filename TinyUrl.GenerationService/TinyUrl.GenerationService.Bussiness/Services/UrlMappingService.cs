using System.Security.Cryptography;
using System.Text;
using TinyUrl.GenerationService.Infrastructure.Contracts.Requests;
using TinyUrl.GenerationService.Infrastructure.Contracts.Responses;
using TinyUrl.GenerationService.Infrastructure.Mappings;
using TinyUrl.GenerationService.Infrastructure.Repositories;
using TinyUrl.GenerationService.Infrastructure.Services;

namespace TinyUrl.GenerationService.Bussiness.Services
{
    public class UrlMappingService : IUrlMappingService
    {
        private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private readonly IUrlMappingRepository _urlMappingRepository;

        public UrlMappingService(IUrlMappingRepository urlMappingRepository)
        {
            _urlMappingRepository = urlMappingRepository;
        }

        public async Task<UrlMappingContract> ShortenUrlAsync(ShortenUrlRequest request, int userId)
        {
            var shortUrl = GenerateShortUrl(request.LongUrl!);
            while (await _urlMappingRepository.IsUrlDublicatedAsync(shortUrl).ConfigureAwait(false))
            {
                shortUrl = GenerateShortUrl(request.LongUrl! + Guid.NewGuid().ToString());
            }


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
