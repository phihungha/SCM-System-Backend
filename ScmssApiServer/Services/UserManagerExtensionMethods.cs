using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Models;

namespace ScmssApiServer.Services
{
    public static class UserManagerExtensionMethods
    {
        public static async Task<User> FindFullUserByIdAsync(
            this UserManager<User> manager,
            string id)
        {
            User user = await manager.Users.Include(i => i.ProductionFacility)
                                           .FirstAsync(i => i.Id == id);
            user.Roles = await manager.GetRolesAsync(user);
            return user;
        }

        /// <summary>
        /// Get a list of role names the specified user belongs to.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Role names</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public static async Task<IList<string>> GetRolesAsync(
            this UserManager<User> manager,
            string userId)
        {
            User user = await manager.FindFullUserByIdAsync(userId);
            return user.Roles;
        }

        /// <summary>
        /// Return True if a user is in any of the specified roles.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="rolesToCheck">Roles to check</param>
        /// <returns>True if user has a role in the specified roles.
        /// False otherwise.</returns>
        public static async Task<bool> IsInAnyRolesAsync(
            this UserManager<User> manager,
            string userId,
            params string[] rolesToCheck)
        {
            IList<string> roles = await manager.GetRolesAsync(userId);
            return roles.Any(i => rolesToCheck.Contains(i));
        }
    }
}
