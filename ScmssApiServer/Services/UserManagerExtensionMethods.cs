using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Models;

namespace ScmssApiServer.Services
{
    public static class UserManagerExtensionMethods
    {
        /// <summary>
        /// Find by ID and return a User with populated Roles.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns></returns>
        public static async Task<User> FindFullUserByIdAsync(this UserManager<User> manager, string id)
        {
            User user = await manager.Users.FirstAsync(i => i.Id == id);
            user.Roles = await manager.GetRolesAsync(user);
            return user;
        }
    }
}
