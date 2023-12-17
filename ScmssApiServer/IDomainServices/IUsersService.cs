using ScmssApiServer.DTOs;
using ScmssApiServer.Services;

namespace ScmssApiServer.IDomainServices
{
    public interface IUsersService
    {
        Task ChangePasswordAsync(string id, UserPasswordChangeDto dto);

        Task<UserDto> CreateAsync(UserCreateDto dto);

        string GenerateProfileImageUploadUrl(string id);

        Task<UserDto?> GetAsync(string id);

        Task<IList<UserDto>> GetManyAsync(SimpleQueryDto dto);

        Task<UserDto> UpdateAsync(string id, UserInputDto dto);
    }
}
