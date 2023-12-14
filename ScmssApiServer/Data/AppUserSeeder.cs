using Microsoft.AspNetCore.Identity;
using ScmssApiServer.Exceptions;
using ScmssApiServer.Models;

namespace ScmssApiServer.Data
{
    public static class AppUserSeeder
    {
        public static readonly string[] Roles =
        {
            "Admin",
            "Director",
            "Finance",
            "SalesSpecialist",
            "SalesManager",
            "PurchaseSpecialist",
            "PurchaseManager",
            "PurchaseManager",
            "ProductionPlanner",
            "ProductionManager",
            "InventorySpecialist",
            "InventoryManager",
            "LogisticsSpecialist",
        };

        public static void SeedRoles(IServiceScope scope, WebApplication app)
        {
            ILogger logger = app.Logger;

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (string role in Roles)
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

            var userName = configuration.GetValue<string>("RootAdminUser:UserName");
            var name = configuration.GetValue<string>("RootAdminUser:Name");
            var email = configuration.GetValue<string>("RootAdminUser:Email");
            var password = configuration.GetValue<string>("RootAdminUser:Password");
            var description = configuration.GetValue<string>("RootAdminUser:Description");
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
                IsActive = true,
                DateOfBirth = new DateTime(1970, 1, 1).ToUniversalTime(),
                Description = description,
            };

            IdentityResult createResult = userManager.CreateAsync(newUser, password).Result;
            if (!createResult.Succeeded)
            {
                throw new ApplicationException("Failed to create root admin user.");
            }

            IdentityResult roleResult = userManager.AddToRolesAsync(newUser, Roles).Result;
            if (!roleResult.Succeeded)
            {
                throw new ApplicationException("Failed to assign roles to root admin user.");
            }

            logger.LogInformation("Created initial root admin user.");
        }
    }
}
