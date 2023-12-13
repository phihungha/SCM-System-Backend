using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.Exceptions;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.IServices;
using ScmssApiServer.Models;
using ScmssApiServer.Services;

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

            IdentityResult result = await _userManager.ChangePasswordAsync(
                user,
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

            result = await _userManager.AddToRolesAsync(user, dto.Roles);
            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }

            return await GetUserDtoAsync(user);
        }

        public string GetProfileImageUploadUrl(Identity identity)
        {
            string key = $"user-profile-images/{identity.Id}";
            return _imageService.GenerateUploadUrl(key);
        }

        public async Task<UserDto?> GetUserAsync(string id)
        {
            User? user = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

            if (user == null)
            {
                return null;
            }

            return await GetUserDtoAsync(user);
        }

        public async Task<IList<UserDto>> GetUsersAsync()
        {
            IList<User> users = await _userManager.Users.AsNoTracking().ToListAsync();
            return _mapper.Map<IList<UserDto>>(users);
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

            IList<string> roles = await _userManager.GetRolesAsync(user);

            result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }

            result = await _userManager.AddToRolesAsync(user, dto.Roles);
            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = dto.Roles;
            return userDto;
        }

        private async Task<UserDto> GetUserDtoAsync(User user)
        {
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = await _userManager.GetRolesAsync(user);
            return userDto;
        }
    }
}
