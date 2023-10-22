using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainServices;
using ScmssApiServer.Exceptions;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.IServices;
using ScmssApiServer.Models;
using ScmssApiServer.Services;

namespace ScmssApiServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            using ILoggerFactory loggerFactory = LoggerFactory.Create(
                loggingBuilder => loggingBuilder
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddConsole());
            ILogger logger = loggerFactory.CreateLogger<Program>();

            // Add external services.
            string? dbConnectionString = builder.Configuration.GetConnectionString("AppDb");
            builder.Services.AddDbContext<ApplicationDbContext>(
                    options => options.UseNpgsql(dbConnectionString)
                );
            AddAuthentication(builder);
            AddAwsS3(builder, logger);
            // Add infrastructure services
            builder.Services.AddSingleton<IImageHostService, ImageHostService>();

            // Add domain services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            ApplicationDbSeeder.SeedRootAdminUser(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        /// <summary>
        /// Setup and add authentication service.
        /// </summary>
        /// <param name="builder">Web application builder</param>
        private static void AddAuthentication(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>(options =>
                    {
                        options.User.RequireUniqueEmail = true;
                    }
                )
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                        {
                            options.SlidingExpiration = true;
                        }
                    );
        }

        /// <summary>
        /// Add AWS S3 client service.
        /// </summary>
        /// <param name="builder">Web application builder</param>
        /// <param name="logger">Logger</param>
        private static void AddAwsS3(WebApplicationBuilder builder, ILogger logger)
        {
            try
            {
                AWSCredentials? credentials = LoadAwsCredentials();
                builder.Services.AddSingleton<IAmazonS3>(i => new AmazonS3Client(credentials));
            }
            catch (AppConfigException ex)
            {
                if (builder.Environment.IsDevelopment())
                {
                    builder.Services.AddSingleton<IAmazonS3>(i => new AmazonS3Client());
                    logger.LogWarning(ex.Message);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Load AWS credentials.
        /// </summary>
        /// <returns>AWS credentials. Null if none found.</returns>
        private static AWSCredentials? LoadAwsCredentials()
        {
            string? profileName = Environment.GetEnvironmentVariable("AWS_PROFILE");
            var chain = new CredentialProfileStoreChain();
            if (!chain.TryGetAWSCredentials(profileName, out var credentials))
                throw new AppConfigException($"Failed to load AWS credentials for profile {profileName}");
            return credentials;
        }
    }
}
