using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.Exceptions;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.IServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IImageHostService _imageService;
        private readonly UserManager<User> _userManager;

        public UsersService(UserManager<User> userManager,
                            IMapper mapper,
                            ApplicationDbContext dbContext,
                            IImageHostService imageService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _imageService = imageService;
            _userManager = userManager;
        }

        public async Task<IList<UserDto>> GetUsersAsync()
        {
            IList<User> users = await _dbContext.Users.Include(i => i.Position)
                                                      .ToListAsync();
            return _mapper.Map<IList<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserAsync(string id)
        {
            User? user = await _dbContext.Users.Include(i => i.Position)
                                         .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<UserDto?>(user);
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto dto)
        {
            var newUser = _mapper.Map<User>(dto);

            IdentityResult? result = await _userManager.CreateAsync(newUser, dto.Password);
            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }

            return _mapper.Map<UserDto>(newUser);
        }

        public async Task<UserDto> UpdateUserAsync(string id, UserUpdateDto dto)
        {
            User? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }

            _mapper.Map(dto, user);

            IdentityResult? result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            User? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            user.IsActive = false;
            await _userManager.UpdateAsync(user);
        }

        public string GetProfileImageUploadUrl(string userId)
        {
            string key = $"user-profile-images/{userId}";
            return _imageService.GenerateUploadUrl(key);
        }
    }
}
