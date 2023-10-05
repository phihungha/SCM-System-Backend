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

            // Add services to the container.
            string? dbConnStr = Environment.GetEnvironmentVariable("DB_CONN");
            builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(dbConnStr)
                );

            builder.Services.AddScoped<IUsersService, UsersService>();

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
    }
}