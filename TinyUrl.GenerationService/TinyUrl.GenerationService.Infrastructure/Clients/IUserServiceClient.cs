using TinyUrl.GenerationService.Infrastructure.Contracts.Responses;

namespace TinyUrl.GenerationService.Infrastructure.Clients
{
    public interface IUserServiceClient
    {
        Task<UserContract> GetUserByIdAsync(int userId);
    }
}
