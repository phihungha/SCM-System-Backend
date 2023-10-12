using Microsoft.EntityFrameworkCore;
using SCM_System_Api_Server.Infrastructure;
using SCM_System_Api_Server.Models;

namespace SCM_System_Api_Server.DomainServices
{
    public class UsersService : IUsersService
    {
        private AppDbContext _dbContext;
        private IImageService _imageService;

        public UsersService(AppDbContext dbContext, IImageService imageService)
        {
            _dbContext = dbContext;
            _imageService = imageService;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public string GetUserProfileImageUploadUrl(long userId)
        {
            string key = $"user-profile-images/{userId}";
            return _imageService.GetImageUploadUrl(key);
        }
    }
}