using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ScmssApiServer.Models;

namespace ScmssApiServer.Data
{
    /// <summary>
    /// Database context for this app.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Position> Positions { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            ChangeTracker.Tracked += ChangeTracker_Tracked;
            ChangeTracker.StateChanged += ChangeTracker_StateChanged;
        }

        /// <summary>
        /// Set creation info on new update-trackable entity.
        /// </summary>
        private void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
        {
            EntityEntry entry = e.Entry;
            if (!e.FromQuery && entry.State == EntityState.Added
                && entry.Entity is IUpdateTrackable entity)
            {
                entity.CreatedTime = DateTime.UtcNow;
                entity.IsActive = true;
            }
        }

        /// <summary>
        /// Set update info on updated update-trackable entity.
        /// </summary>
        private void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
        {
            EntityEntry entry = e.Entry;

            if (!(entry.Entity is IUpdateTrackable entity))
            {
                return;
            }

            if (e.NewState == EntityState.Modified || e.NewState == EntityState.Deleted)
            {
                entity.UpdatedTime = DateTime.UtcNow;
            }
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder
                .Properties<Gender>()
                .HaveConversion<string>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Position>()
                .HasMany(i => i.Permissions)
                .WithMany(i => i.Positions)
                .UsingEntity<PositionPermission>();

            builder.Entity<Permission>().HasData(
                new Permission
                {
                    Id = "admin",
                    DisplayName = "Administration",
                    Description = "Full permissions"
                });

            builder.Entity<Position>().HasData(
                new Position
                {
                    Id = 1,
                    Name = "Administrator",
                    Description = "Administrator for the system",
                    IsActive = true,
                    CreatedTime = DateTime.UtcNow
                });

            builder.Entity<PositionPermission>().HasData(
                new PositionPermission()
                {
                    PermissionId = "admin",
                    PositionId = 1,
                });
        }
    }
}
