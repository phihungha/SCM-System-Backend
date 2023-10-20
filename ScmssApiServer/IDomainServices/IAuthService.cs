using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IAuthService
    {
        Task<bool> SignInAsync(AuthSignInDto dto);

        Task SignOutAsync();
    }
}
