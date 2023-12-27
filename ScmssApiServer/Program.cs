using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
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
            AddCors(builder);
            AddDb(builder);
            AddAuthentication(builder);
            AddAwsS3(builder, logger);
            builder.Services.AddProblemDetails(o =>
            {
                o.IncludeExceptionDetails = (ctx, ex) => builder.Environment.IsDevelopment();
                o.MapToStatusCode<EntityNotFoundException>(StatusCodes.Status404NotFound);
                o.MapToStatusCode<InvalidDomainOperationException>(StatusCodes.Status400BadRequest);
                o.MapToStatusCode<UnauthorizedException>(StatusCodes.Status403Forbidden);
                o.MapToStatusCode<UnauthenticatedException>(StatusCodes.Status401Unauthorized);
            });

            // Add infrastructure services
            builder.Services.AddScoped<IClaimsTransformation, CustomClaimsTransformation>();
            builder.Services.AddSingleton<IFileHostService, FileHostService>();

            // Add domain services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IConfigService, ConfigService>();
            builder.Services.AddScoped<ICustomersService, CustomersService>();
            builder.Services.AddScoped<IInventoryService, InventoryService>();
            builder.Services.AddScoped<ISalesOrdersService, SalesOrdersService>();
            builder.Services.AddScoped<ISuppliesService, SuppliesService>();
            builder.Services.AddScoped<IProductsService, ProductsService>();
            builder.Services.AddScoped<IProductionOrdersService, ProductionOrdersService>();
            builder.Services.AddScoped<IProductionFacilitiesService, ProductionFacilitiesService>();
            builder.Services.AddScoped<IPurchaseRequisitionsService, PurchaseRequisitionsService>();
            builder.Services.AddScoped<IPurchaseOrdersService, PurchaseOrdersService>();
            builder.Services.AddScoped<IReportsService, ReportsService>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IVendorsService, VendorsService>();

            builder.Services.AddControllers()
                .AddProblemDetailsConventions()
                .AddJsonOptions(
                    o => o.JsonSerializerOptions
                          .Converters
                          .Add(new JsonStringEnumConverter()));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                AppUserSeeder.SeedRoles(scope, app);
                AppUserSeeder.SeedRootAdminUser(scope, app);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseProblemDetails();

            app.UseCors(CorsPolicyName);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        /// <summary>
        /// Setup and add authentication service.
        /// </summary>
        private static void AddAuthentication(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>(options =>
                    {
                        options.User.RequireUniqueEmail = true;
                    }
                )
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                    {
                        options.SlidingExpiration = true;
                    }
                );
        }

        /// <summary>
        /// Add AWS S3 client service.
        /// </summary>
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
        /// Setup and add CORS.
        /// </summary>
        private static void AddCors(WebApplicationBuilder builder)
        {
            string? clientUrl = builder.Configuration.GetValue<string>("Cors:ClientUrl");
            if (clientUrl != null)
            {
                builder.Services.AddCors(o => o.AddPolicy(
                    name: CorsPolicyName,
                    builder => builder.WithOrigins(clientUrl)
                                      .WithHeaders(HeaderNames.ContentType)
                                      .WithMethods("GET", "POST", "PATCH", "PUT")
                                      .AllowCredentials())
                );
            }
        }

        /// <summary>
        /// Setup and add database service.
        /// </summary>
        private static void AddDb(WebApplicationBuilder builder)
        {
            string? dbConnectionString = builder.Configuration.GetConnectionString("AppDb");
            builder.Services.AddDbContext<AppDbContext>(
                    o => o.UseNpgsql(
                        dbConnectionString,
                        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                );
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
