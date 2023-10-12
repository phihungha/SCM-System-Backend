using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using SCM_System_Api_Server.DomainServices;
using SCM_System_Api_Server.Infrastructure;

namespace SCM_System_Api_Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add external services.
            string? dbConnectionString = Environment.GetEnvironmentVariable("DB_CONN");
            builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(dbConnectionString)
                );
            AWSCredentials ssoCredential = LoadSsoCredential();
            builder.Services.AddSingleton<IAmazonS3>(i => new AmazonS3Client(ssoCredential));

            // Add infrastructure services
            builder.Services.AddSingleton<IImageService, ImageService>();

            // Add domain services
            builder.Services.AddScoped<IUsersService, UsersService>();

            // Add controllers
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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

        private static AWSCredentials LoadSsoCredential()
        {
            string? awsProfile = Environment.GetEnvironmentVariable("AWS_PROFILE");
            var chain = new CredentialProfileStoreChain();
            if (!chain.TryGetAWSCredentials(awsProfile, out var credentials))
                throw new Exception($"Failed to find the {awsProfile} profile");
            return credentials;
        }
    }
}