using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IAuthService
    {
        Task<UserDto?> SignInAsync(AuthSignInDto dto);

        Task SignOutAsync();
    }
}
