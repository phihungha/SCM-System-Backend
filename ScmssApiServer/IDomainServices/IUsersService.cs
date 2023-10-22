using ScmssApiServer.DTOs;
using ScmssApiServer.Models;

namespace ScmssApiServer.IDomainServices
{
    public interface IUsersService
    {
        Task<IList<User>> GetUsersAsync();

        Task<User?> GetUserAsync(string id);

        Task<User> CreateUserAsync(UserCreateDto dto);

        string GetProfileImageUploadUrl(string userId);
    }
}
