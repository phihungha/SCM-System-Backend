using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ScmssApiServer.Models;

namespace ScmssApiServer.Data
{
    /// <summary>
    /// Database context for this app.
    /// </summary>
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<PurchaseRequisition> PurchaseRequisitions { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Supply> Supplies { get; set; }

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
                entity.CreateTime = DateTime.UtcNow;
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
                entity.UpdateTime = DateTime.UtcNow;
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

            #region PurchaseRequisition
            // Many-to-many with Supply via PurchaseRequisitionItem
            builder.Entity<PurchaseRequisition>()
                .HasMany(e => e.Supplies)
                .WithMany(e => e.PurchaseRequisitions)
                .UsingEntity<PurchaseRequisitionItem>();

            builder.Entity<PurchaseRequisition>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.CreatedPurchaseRequisitions)
                .HasForeignKey(e => e.CreateUserId);

            builder.Entity<PurchaseRequisition>()
                .HasOne(e => e.ApproveProductionManager)
                .WithMany(e => e.ApprovedPurchaseRequisitionsAsManager)
                .HasForeignKey(e => e.ApproveProductionManagerId);

            builder.Entity<PurchaseRequisition>()
                .HasOne(e => e.ApproveFinance)
                .WithMany(e => e.ApprovedPurchaseRequisitionsAsFinance)
                .HasForeignKey(e => e.ApproveFinanceId);

            builder.Entity<PurchaseRequisition>()
                .HasOne(e => e.FinishUser)
                .WithMany(e => e.FinishedPurchaseRequisitions)
                .HasForeignKey(e => e.FinishUserId);
            #endregion

            #region PurchaseOrder
            // Many-to-many with Supply via PurchaseOrderItem
            builder.Entity<PurchaseOrder>()
                .HasMany(e => e.Supplies)
                .WithMany(e => e.PurchaseOrders)
                .UsingEntity<PurchaseOrderItem>();

            builder.Entity<PurchaseOrder>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.CreatedPurchaseOrders)
                .HasForeignKey(e => e.CreateUserId);

            builder.Entity<PurchaseOrder>()
                .HasOne(e => e.FinishUser)
                .WithMany(e => e.FinishedPurchaseOrders)
                .HasForeignKey(e => e.FinishUserId);
            #endregion

        }
    }
}
