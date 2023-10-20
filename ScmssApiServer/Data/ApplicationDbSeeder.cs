using Microsoft.AspNetCore.Identity;
using ScmssApiServer.Exceptions;
using ScmssApiServer.Models;

namespace ScmssApiServer.Data
{
    public static class ApplicationDbSeeder
    {
        /// <summary>
        /// Create initial root admin user if it doesn't exist yet.
        /// </summary>
        /// <param name="userManager">User manager</param>
        /// <exception cref="Exception">Failed to create root admin user</exception>
        public static void SeedRootAdminUser(UserManager<User> userManager,
                                             WebApplication app)
        {
            ILogger logger = app.Logger;
            IConfiguration configuration = app.Configuration;

            var userName = configuration.GetValue<string>("InitialRootAdminUser:UserName");
            var name = configuration.GetValue<string>("InitialRootAdminUser:Name");
            var email = configuration.GetValue<string>("InitialRootAdminUser:Email");
            var password = configuration.GetValue<string>("InitialRootAdminUser:Password");
            var description = configuration.GetValue<string>("InitialRootAdminUser:Description");
            if (userName == null
                || name == null
                || email == null
                || password == null
                || description == null)
            {
                if (app.Environment.IsDevelopment())
                {
                    logger.LogWarning("Initial root admin user is not properly configured.");
                    return;
                }
                else
                {
                    throw new AppConfigException("Initial root admin user is not properly configured.");
                }
            }

            User? user = userManager.FindByNameAsync(userName).Result;
            if (user != null)
            {
                return;
            }

            var newUser = new User()
            {
                UserName = userName,
                Email = email,
                Name = name,
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1970, 1, 1).ToUniversalTime(),
                IdCardNumber = "000000000000",
                Description = ""
            };

            IdentityResult result = userManager.CreateAsync(newUser, password).Result;

            if (!result.Succeeded)
            {
                throw new ApplicationException("Failed to create root admin user.");
            }
        }
    }
}
