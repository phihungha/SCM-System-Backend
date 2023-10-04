using Microsoft.EntityFrameworkCore;
using SCM_System_Api_Server.Infrastructure;
using SCM_System_Api_Server.Models;

namespace SCM_System_Api_Server.DomainServices
{
    public class UsersService : IUsersService
    {
        private AppDbContext _dbContext;

        public UsersService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }
    }
}