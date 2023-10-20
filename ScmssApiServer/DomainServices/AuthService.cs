using Microsoft.AspNetCore.Identity;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<User> _signInManager;

        public AuthService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<bool> SignInAsync(AuthSignInDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(
                dto.UserName, dto.Password, isPersistent: true, lockoutOnFailure: false
                );

            if (!result.Succeeded)
            {
                return false;
            }
            return true;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
