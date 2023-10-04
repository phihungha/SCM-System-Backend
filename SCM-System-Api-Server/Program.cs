using Microsoft.EntityFrameworkCore;
using SCM_System_Api_Server.Data;

namespace SCM_System_Api_Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(GetDbConnectionString())
                );
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

        public static string GetDbConnectionString()
        {
            string? dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            string? dbName = Environment.GetEnvironmentVariable("DB_NAME");
            string? dbUser = Environment.GetEnvironmentVariable("DB_USER");
            string? dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            return $"Host={dbHost};Database={dbName};Username={dbUser};Password={dbPassword}";
        }
    }
}