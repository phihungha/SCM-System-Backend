using ScmssApiServer.DTOs;
using System.Security.Claims;

namespace ScmssApiServer.IDomainServices
{
    public interface IUsersService
    {
        string? GetUserIdFromPrincipal(ClaimsPrincipal principal);

        Task<IList<UserDto>> GetUsersAsync();

        Task<UserDto?> GetUserAsync(string id);

        Task<UserDto> CreateUserAsync(UserCreateDto dto);

        Task<UserDto> UpdateUserAsync(string id, UserInputDto dto);

        Task ChangePasswordAsync(string id, UserPasswordChangeDto dto);

        string GetProfileImageUploadUrl(string userId);
    }
}
