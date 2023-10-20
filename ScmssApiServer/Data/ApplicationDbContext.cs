using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ScmssApiServer.Models;

namespace ScmssApiServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            ChangeTracker.Tracked += ChangeTracker_Tracked;
            ChangeTracker.StateChanged += ChangeTracker_StateChanged;
        }

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

            if (e.NewState == EntityState.Deleted)
            {
                entity.IsActive = false;
            }
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder
                .Properties<Gender>()
                .HaveConversion<string>();
        }
    }
}
