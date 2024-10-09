using Microsoft.Extensions.Options;
using System.Text.Json;
using TinyUrl.GenerationService.Infrastructure.Clients;
using TinyUrl.GenerationService.Infrastructure.Contracts.Options;
using TinyUrl.GenerationService.Infrastructure.Contracts.Responses;

namespace TinyUrl.GenerationService.Data.Clients
{
    public class UserServiceClient : IUserServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly UserClientOptions _options;

        public UserServiceClient(HttpClient httpClient, IOptions<UserClientOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<UserContract> GetUserByIdAsync(int userId)
        {
            UserContract result = null;

            var url = _options.BaseUrl + $"users/{userId}";

            using HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();

                result =  JsonSerializer.Deserialize<UserContract>(jsonResult)!;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null!;
                }
            }

            return result!;
        }
    }
}
