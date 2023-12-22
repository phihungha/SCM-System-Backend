using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthService(IMapper mapper,
                           SignInManager<User> signInManager,
                           UserManager<User> usersManager)
        {
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = usersManager;
        }

        public async Task<UserDto?> SignInAsync(AuthSignInDto dto)
        {
            var result = await _signInManager.PasswordSignInAsync(
                dto.UserName, dto.Password, isPersistent: true, lockoutOnFailure: false
                );

            if (!result.Succeeded)
            {
                return null;
            }

            User user = await _userManager.Users.AsNoTracking()
                                                .Include(i => i.ProductionFacility)
                                                .SingleAsync(i => i.UserName == dto.UserName);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = await _userManager.GetRolesAsync(user);
            return userDto;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
