using ScmssApiServer.DTOs;
using ScmssApiServer.Services;

namespace ScmssApiServer.IDomainServices
{
    public interface IUsersService
    {
        Task ChangePasswordAsync(string id, UserPasswordChangeDto dto);

        Task<UserDto> CreateUserAsync(UserCreateDto dto);

        string GetProfileImageUploadUrl(Identity identity);

        Task<UserDto?> GetUserAsync(string id);

        Task<IList<UserDto>> GetUsersAsync();

        Task<UserDto> UpdateUserAsync(string id, UserInputDto dto);
    }
}
