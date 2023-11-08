using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IUsersService
    {
        Task<IList<UserDto>> GetUsersAsync();

        Task<UserDto?> GetUserAsync(string id);

        Task<UserDto> CreateUserAsync(UserCreateDto dto);

        Task<UserDto> UpdateUserAsync(string id, UserInputDto dto);

        Task ChangePasswordAsync(string id, UserPasswordChangeDto dto);

        Task DeleteUserAsync(string id);

        string GetProfileImageUploadUrl(string userId);
    }
}
