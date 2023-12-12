using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.Exceptions;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.IServices;
using ScmssApiServer.Models;
using System.Security.Claims;

namespace ScmssApiServer.DomainServices
{
    public class UsersService : IUsersService
    {
        private readonly IImageHostService _imageService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UsersService(UserManager<User> userManager,
                            IMapper mapper,
                            IImageHostService imageService)
        {
            _mapper = mapper;
            _imageService = imageService;
            _userManager = userManager;
        }

        public async Task ChangePasswordAsync(string id, UserPasswordChangeDto dto)
        {
            User? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }

            IdentityResult result = await _userManager
                .ChangePasswordAsync(user,
                                     dto.CurrentPassword,
                                     dto.NewPassword);
            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto dto)
        {
            var user = _mapper.Map<User>(dto);

            IdentityResult result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }

            return GetUserDto(user);
        }

        public string GetProfileImageUploadUrl(string userId)
        {
            string key = $"user-profile-images/{userId}";
            return _imageService.GenerateUploadUrl(key);
        }

        public async Task<UserDto?> GetUserAsync(string id)
        {
            User? user = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = await _userManager.GetRolesAsync(user);
            return userDto;
        }

        public string? GetUserIdFromPrincipal(ClaimsPrincipal principal)
        {
            return _userManager.GetUserId(principal);
        }

        public async Task<IList<UserDto>> GetUsersAsync()
        {
            IList<User> users = await _userManager.Users.AsNoTracking().ToListAsync();

            var userDtos = _mapper.Map<IList<UserDto>>(users);

            for (int i = 0; i < users.Count; i++)
            {
                userDtos[i].Roles = await _userManager.GetRolesAsync(users[i]);
            }

            return userDtos;
        }

        public async Task<UserDto> UpdateUserAsync(string id, UserInputDto dto)
        {
            User? user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }

            _mapper.Map(dto, user);

            IdentityResult result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }

            return GetUserDto(user);
        }

        private UserDto GetUserDto(User user)
        {
            return _mapper.Map<UserDto>(user);
        }
    }
}
