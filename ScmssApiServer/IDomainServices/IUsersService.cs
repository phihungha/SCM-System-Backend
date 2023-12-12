using ScmssApiServer.DTOs;
using System.Security.Claims;

namespace ScmssApiServer.IDomainServices
{
    public interface IUsersService
    {
        Task ChangePasswordAsync(string id, UserPasswordChangeDto dto);

        Task<UserDto> CreateUserAsync(UserCreateDto dto);

        string GetProfileImageUploadUrl(string userId);

        Task<UserDto?> GetUserAsync(string id);

        string? GetUserIdFromPrincipal(ClaimsPrincipal principal);

        Task<IList<UserDto>> GetUsersAsync();

        Task<UserDto> UpdateUserAsync(string id, UserInputDto dto);
    }
}
