using Microsoft.EntityFrameworkCore;
using SCM_System_Api_Server.Models;

namespace SCM_System_Api_Server.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}