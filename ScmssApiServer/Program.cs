using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ScmssApiServer.Data;
using ScmssApiServer.DomainServices;
using ScmssApiServer.Exceptions;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.IServices;
using ScmssApiServer.Models;
using ScmssApiServer.Services;
using System.Text.Json.Serialization;

namespace ScmssApiServer
{
    public class Program
    {
        private const string CorsPolicyName = "Main";

        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            using ILoggerFactory loggerFactory = LoggerFactory.Create(
                loggingBuilder => loggingBuilder
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddConsole());
            ILogger logger = loggerFactory.CreateLogger<Program>();

            // Add external services.
            builder.Services.AddAutoMapper(typeof(Program));

            string? dbConnectionString = builder.Configuration.GetConnectionString("AppDb");
            builder.Services.AddDbContext<AppDbContext>(
                    o => o.UseNpgsql(dbConnectionString)
                );

            AddAuthentication(builder);
            AddAwsS3(builder, logger);

            // Add infrastructure services
            builder.Services.AddSingleton<IImageHostService, ImageHostService>();

            // Add domain services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<ISalesOrdersService, SalesOrdersService>();
            builder.Services.AddScoped<IProductionOrdersService, ProductionOrdersService>();
            builder.Services.AddScoped<IPurchaseRequisitionsService, PurchaseRequisitionsService>();

            builder.Services.AddCors(o => o.AddPolicy(
                name: CorsPolicyName,
                builder => builder.WithHeaders(HeaderNames.ContentType)
                                  .AllowCredentials()
                                  .WithOrigins("http://localhost:3000"))
            );

            builder.Services.AddControllers().AddJsonOptions(
                    o => o.JsonSerializerOptions
                          .Converters
                          .Add(new JsonStringEnumConverter())
                );
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            AppDbSeeder.SeedRootAdminUser(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(CorsPolicyName);

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
                .AddEntityFrameworkStores<AppDbContext>();
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
