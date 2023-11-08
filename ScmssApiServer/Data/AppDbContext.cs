using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;
using ScmssApiServer.Models;
using System.Reflection.Emit;

namespace ScmssApiServer.Data
{
    /// <summary>
    /// Database context for this app.
    /// </summary>
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
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

            builder
                .Properties<OrderStatus>()
                .HaveConversion<string>();

            builder
                .Properties<OrderPaymentStatus>()
                .HaveConversion<string>();

            builder
                .Properties<PurchaseRequisitionStatus>()
                .HaveConversion<string>();


        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
