using Microsoft.AspNetCore.Identity;
using ScmssApiServer.Exceptions;
using ScmssApiServer.Models;

namespace ScmssApiServer.Data
{
    public static class AppDbSeeder
    {
        public static void SeedRoles(IServiceScope scope, WebApplication app)
        {
            ILogger logger = app.Logger;

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new string[]
            {
                "Admin",
                "Director",
                "Finance",
                "SalesSpecialist",
                "SalesManager",
                "PurchaseSpecialist",
                "PurchaseManager",
                "ProductionPlanner",
                "ProductionManager",
                "LogisticsSpecialist",
            };

            foreach (string role in roles)
            {
                if (roleManager.FindByNameAsync(role).Result != null)
                {
                    continue;
                }

                IdentityResult result = roleManager.CreateAsync(new IdentityRole(role)).Result;
                if (!result.Succeeded)
                {
                    throw new ApplicationException($"Failed to create role {role}.");
                }
            }

            logger.LogInformation("Created roles.");
        }

        /// <summary>
        /// Create initial root admin user if it doesn't exist yet.
        /// </summary>
        /// <param name="userManager">User manager</param>
        /// <exception cref="Exception">Failed to create root admin user</exception>
        public static void SeedRootAdminUser(IServiceScope scope, WebApplication app)
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

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

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
                ProductionFacilityId = 1,
                DateOfBirth = new DateTime(1970, 1, 1).ToUniversalTime(),
                Description = description,
            };

            IdentityResult createResult = userManager.CreateAsync(newUser, password).Result;
            if (!createResult.Succeeded)
            {
                throw new ApplicationException("Failed to create root admin user.");
            }

            IdentityResult roleResult = userManager.AddToRoleAsync(newUser, "Admin").Result;
            if (!roleResult.Succeeded)
            {
                throw new ApplicationException("Failed to assign roles to root admin user.");
            }

            logger.LogInformation("Created initial root admin user.");
        }
    }
}
