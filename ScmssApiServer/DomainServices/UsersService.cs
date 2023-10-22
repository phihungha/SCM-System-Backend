using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
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
        private readonly IImageHostService _imageService;
        private readonly UserManager<User> _userManager;

        public UsersService(UserManager<User> userManager,
                            ApplicationDbContext dbContext,
                            IImageHostService imageService)
        {
            _dbContext = dbContext;
            _imageService = imageService;
            _userManager = userManager;
        }

        public async Task<IList<User>> GetUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetUserAsync(string id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> CreateUserAsync(UserCreateDto dto)
        {
            var newUser = new User()
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Name = dto.Name,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                IdCardNumber = dto.IdCardNumber,
                Address = dto.Address,
                Description = dto.Description
            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);

            if (!result.Succeeded)
            {
                throw new IdentityException(result);
            }

            return newUser;
        }

        public string GetProfileImageUploadUrl(string userId)
        {
            string key = $"user-profile-images/{userId}";
            return _imageService.GenerateUploadUrl(key);
        }
    }
}
