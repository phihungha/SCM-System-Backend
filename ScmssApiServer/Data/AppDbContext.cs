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
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            ChangeTracker.Tracked += ChangeTracker_Tracked;
            ChangeTracker.StateChanged += ChangeTracker_StateChanged;
        }

        public DbSet<Config> Config { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ProductionFacility> ProductionFacilities { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseRequisition> PurchaseRequisitions { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder
                .Properties<Gender>()
                .HaveConversion<string>();

            builder
                .Properties<Uri>()
                .HaveConversion<string>();

            builder
                .Properties<OrderStatus>()
                .HaveConversion<string>();

            builder
                .Properties<TransOrderPaymentStatus>()
                .HaveConversion<string>();

            builder
                .Properties<PurchaseRequisitionStatus>()
                .HaveConversion<string>();

            builder
                .Properties<ProductionOrderStatus>()
                .HaveConversion<string>();

            builder
                .Properties<TransOrderEventType>()
                .HaveConversion<string>();

            builder
                .Properties<ProductionOrderEventType>()
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
                .UsingEntity<PurchaseRequisitionItem>(
                    l => l.HasOne(e => e.Supply)
                    .WithMany(e => e.PurchaseRequisitionItems)
                    .HasForeignKey(e => e.ItemId),

                    r => r.HasOne(e => e.PurchaseRequisition)
                    .WithMany(e => e.Items)
                    .HasForeignKey(e => e.OrderId)
                );

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
                .HasOne(e => e.EndUser)
                .WithMany(e => e.FinishedPurchaseRequisitions)
                .HasForeignKey(e => e.EndUserId);

            #endregion PurchaseRequisition

            #region PurchaseOrder

            // Many-to-many with Supply via PurchaseOrderItem
            builder.Entity<PurchaseOrder>()
                .HasMany(e => e.Supplies)
                .WithMany(e => e.PurchaseOrders)
                .UsingEntity<PurchaseOrderItem>(
                    l => l.HasOne(e => e.Supply)
                    .WithMany(e => e.PurchaseOrderItems)
                    .HasForeignKey(e => e.ItemId),

                    r => r.HasOne(e => e.PurchaseOrder)
                    .WithMany(e => e.Items)
                    .HasForeignKey(e => e.OrderId)
                );

            builder.Entity<PurchaseOrder>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.CreatedPurchaseOrders)
                .HasForeignKey(e => e.CreateUserId);

            builder.Entity<PurchaseOrder>()
                .HasOne(e => e.EndUser)
                .WithMany(e => e.FinishedPurchaseOrders)
                .HasForeignKey(e => e.EndUserId);

            #endregion PurchaseOrder

            #region SalesOrder

            // Many-to-many with Supply via PurchaseOrderItem
            builder.Entity<SalesOrder>()
                .HasMany(e => e.Products)
                .WithMany(e => e.SalesOrders)
                .UsingEntity<SalesOrderItem>(
                    l => l.HasOne(e => e.Product)
                    .WithMany(e => e.SalesOrderItems)
                    .HasForeignKey(e => e.ItemId),

                    r => r.HasOne(e => e.SalesOrder)
                    .WithMany(e => e.Items)
                    .HasForeignKey(e => e.OrderId)
                );

            builder.Entity<SalesOrder>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.CreatedSalesOrders)
                .HasForeignKey(e => e.CreateUserId);

            builder.Entity<SalesOrder>()
                .HasOne(e => e.EndUser)
                .WithMany(e => e.FinishedSalesOrders)
                .HasForeignKey(e => e.EndUserId);

            #endregion SalesOrder

            #region Product

            builder.Entity<Product>()
                .HasMany(e => e.Supplies)
                .WithMany(e => e.Products)
                .UsingEntity<ProductSupplyCostItem>();

            #endregion Product

            #region ProductionOrder

            // Many-to-many with Product via ProductionOrderItem
            builder.Entity<ProductionOrder>()
                .HasMany(e => e.Products)
                .WithMany(e => e.ProductionOrders)
                .UsingEntity<ProductionOrderItem>(
                    l => l.HasOne(e => e.Product)
                    .WithMany(e => e.ProductionOrderItems)
                    .HasForeignKey(e => e.ItemId),
                    r => r.HasOne(e => e.ProductionOrder)
                    .WithMany(e => e.Items)
                    .HasForeignKey(e => e.OrderId)
            );

            builder.Entity<ProductionOrder>()
                .HasMany(e => e.Supplies)
                .WithMany(e => e.ProductionOrders)
                .UsingEntity<ProductionOrderSupplyUsageItem>();

            builder.Entity<ProductionOrder>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.CreatedProductionOrders)
                .HasForeignKey(e => e.CreateUserId);

            builder.Entity<ProductionOrder>()
                .HasOne(e => e.ApproveProductionManager)
                .WithMany(e => e.ApprovedProductionOrdersAsManager)
                .HasForeignKey(e => e.ApproveProductionManagerId);

            builder.Entity<ProductionOrder>()
                .HasOne(e => e.EndUser)
                .WithMany(e => e.FinishedProductionOrders)
                .HasForeignKey(e => e.EndUserId);

            #endregion ProductionOrder

            #region ProductionFacility

            // Many-to-many with Supply via WarehouseSupplyItem
            builder.Entity<ProductionFacility>()
                .HasMany(e => e.WarehouseSupplies)
                .WithMany(e => e.ProductionFacilities)
                .UsingEntity<WarehouseSupplyItem>();

            // Many-to-many with Product via WarehouseProductItem
            builder.Entity<ProductionFacility>()
                .HasMany(e => e.WarehouseProducts)
                .WithMany(e => e.ProductionFacilities)
                .UsingEntity<WarehouseProductItem>();

            #endregion ProductionFacility

            #region Seeding

            builder.Entity<Config>().HasData(new Config { Id = 1, VatRate = 0.05 });

            builder.Entity<Customer>().HasData(
            new Customer
            {
                Id = 1,
                Name = "Cool Garden 324",
                Email = "watarichanno@gmail.com",
                PhoneNumber = "0344250401",
                DefaultLocation = "156 Nguyen Van Luong, Bien Hoa, Dong nai",
                Description = "Flower garden.",
                ContactPerson = "Hoa Thi Mai",
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            },
            new Customer
            {
                Id = 2,
                Name = "Phi Hung Shop",
                Email = "haphihung55@gmail.com",
                PhoneNumber = "0344250401",
                DefaultLocation = "436 Vo Van Kiet, District 1, HCM City",
                Description = "Plant shop.",
                ContactPerson = "Ha Phi Hung",
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            });

            builder.Entity<ProductionFacility>().HasData(
            new ProductionFacility
            {
                Id = 1,
                Name = "Go Dau",
                Description = "Primary production facility",
                Location = "Go Dau Industrial Park, Phuoc Thai, Long Thanh, Dong Nai",
                PhoneNumber = "02837560110",
                Email = "godau@cool-fertilizer.com.vn",
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            },
            new ProductionFacility
            {
                Id = 2,
                Name = "Binh Dien - Long An",
                Description = "Secondary production facility",
                Location = "Long Dinh Industrial Park, Long Dinh, Can Duoc, Long An",
                PhoneNumber = "02723726627",
                Email = "longan@cool-fertilizer.com.vn",
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            });

            builder.Entity<Vendor>().HasData(
            new Vendor
            {
                Id = 1,
                Name = "PVFCCo",
                Email = "customer@pvfcco.com.vn",
                PhoneNumber = "02838256258",
                DefaultLocation = "Phu My Industrial Park, Phu My, Phu My, Ba Ria - Vung Tau",
                Description = "Main vendor for major ingredients.",
                ContactPerson = "Ha Long Anh",
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            },
            new Vendor
            {
                Id = 2,
                Name = "Vinachem",
                Email = "order@vinachem.com.vn",
                PhoneNumber = "02438240551",
                DefaultLocation = "Binh Duong Industrial Park, An Binh, Di An, Binh Duong",
                Description = "Main vendor for trace ingredients.",
                ContactPerson = "Nguyen Thanh Long",
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            });

            builder.Entity<Supply>().HasData(
            new Supply
            {
                Id = 1,
                VendorId = 1,
                Name = "PVFCCo Urea",
                ExpirationMonth = 12,
                Unit = "Kg",
                Price = 5000,
                Description = "CO(NH2)2 for nitrogen.",
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            },
            new Supply
            {
                Id = 2,
                VendorId = 1,
                Name = "PVFCCo Phosphorous",
                ExpirationMonth = 12,
                Unit = "Kg",
                Price = 6000,
                Description = "P2O5 for phosphorous.",
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            },
            new Supply
            {
                Id = 3,
                VendorId = 1,
                Name = "PVFCCo Potassium Chloride",
                ExpirationMonth = 12,
                Unit = "Kg",
                Price = 5000,
                Description = "KCl for potassium.",
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            },
            new Supply
            {
                Id = 4,
                VendorId = 2,
                Name = "Vinachem Ammonium Sulphate",
                ExpirationMonth = 12,
                Unit = "Kg",
                Price = 13000,
                Description = "(NH4)2SO4 for trace sulfur.",
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            },
            new Supply
            {
                Id = 5,
                VendorId = 2,
                Name = "Vinachem Boric Acid",
                ExpirationMonth = 12,
                Unit = "Kg",
                Price = 38000,
                Description = "H3BO3 for trace boron.",
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            },
            new Supply
            {
                Id = 6,
                VendorId = 2,
                Name = "Vinachem Zinc Sulphate",
                ExpirationMonth = 12,
                Unit = "Kg",
                Price = 40000,
                Description = "ZnSO4 for trace zinc.",
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            });

            builder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "NPK 16-8-8+13S",
                NetWeight = 50,
                ExpirationMonth = 48,
                Unit = "Item(s)",
                Price = 500000,
                Description = "MSPB: 04513\n" +
                                "Protein total (Nts): 16%\n" +
                                "Effective Phosphate (P2O5hh): 8%\n" +
                                "Effective Potassium (K2Ohh): 8%\n" +
                                "Sulfur (S): 13%\n" +
                                "Humidity: 2%\n" +
                                "Suitable for all crops.",
                SupplyCostItems =
                {
                },
                MiscCost = 15000,
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            },
            new Product
            {
                Id = 2,
                Name = "NPK 16-7-18+12S+TE",
                NetWeight = 50,
                ExpirationMonth = 48,
                Unit = "Item(s)",
                Price = 600000,
                Description = "MSPB: 04519\n" +
                                "Protein total (Nts): 16%\n" +
                                "Effective Phosphate (P2O5hh): 7%\n" +
                                "Effective Potassium (K2Ohh): 18%\n" +
                                "Sulfur (S): 12%\n" +
                                "Bo (B): 217ppm\n" +
                                "Zinc (Zn): 400ppm\n" +
                                "Humidity: 2%\n" +
                                "Suitable for coffee, fruit, rubber, " +
                                "vegetable, rice crops.",
                SupplyCostItems =
                {
                },
                MiscCost = 18000,
                IsActive = true,
                CreateTime = DateTime.UtcNow,
            });

            builder.Entity<ProductSupplyCostItem>().HasData(
                new ProductSupplyCostItem { ProductId = 1, SupplyId = 1, Quantity = 20.6 },
                new ProductSupplyCostItem { ProductId = 1, SupplyId = 2, Quantity = 8 },
                new ProductSupplyCostItem { ProductId = 1, SupplyId = 3, Quantity = 8 },
                new ProductSupplyCostItem { ProductId = 1, SupplyId = 4, Quantity = 13 },
                new ProductSupplyCostItem { ProductId = 2, SupplyId = 1, Quantity = 16 },
                new ProductSupplyCostItem { ProductId = 2, SupplyId = 2, Quantity = 8 },
                new ProductSupplyCostItem { ProductId = 2, SupplyId = 3, Quantity = 13 },
                new ProductSupplyCostItem { ProductId = 2, SupplyId = 4, Quantity = 10 },
                new ProductSupplyCostItem { ProductId = 2, SupplyId = 5, Quantity = 1.5 },
                new ProductSupplyCostItem { ProductId = 2, SupplyId = 6, Quantity = 1.5 }
            );

            builder.Entity<WarehouseSupplyItem>().HasData(
            new WarehouseSupplyItem
            {
                Id = 1,
                SupplyId = 1,
                ProductionFacilityId = 1,
                Quantity = 13000,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseSupplyItem
            {
                Id = 2,
                SupplyId = 2,
                ProductionFacilityId = 1,
                Quantity = 12500,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseSupplyItem
            {
                Id = 3,
                SupplyId = 3,
                ProductionFacilityId = 1,
                Quantity = 12500,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseSupplyItem
            {
                Id = 4,
                SupplyId = 4,
                ProductionFacilityId = 1,
                Quantity = 12000,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseSupplyItem
            {
                Id = 5,
                SupplyId = 5,
                ProductionFacilityId = 1,
                Quantity = 1800,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseSupplyItem
            {
                Id = 6,
                SupplyId = 6,
                ProductionFacilityId = 1,
                Quantity = 1800,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseSupplyItem
            {
                Id = 7,
                SupplyId = 1,
                ProductionFacilityId = 2,
                Quantity = 12000,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseSupplyItem
            {
                Id = 8,
                SupplyId = 2,
                ProductionFacilityId = 2,
                Quantity = 12000,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseSupplyItem
            {
                Id = 9,
                SupplyId = 3,
                ProductionFacilityId = 2,
                Quantity = 12500,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseSupplyItem
            {
                Id = 10,
                SupplyId = 4,
                ProductionFacilityId = 2,
                Quantity = 11000,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseSupplyItem
            {
                Id = 11,
                SupplyId = 5,
                ProductionFacilityId = 2,
                Quantity = 1500,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseSupplyItem
            {
                Id = 12,
                SupplyId = 6,
                ProductionFacilityId = 2,
                Quantity = 1500,
                CreateTime = DateTime.UtcNow,
            });

            builder.Entity<WarehouseProductItem>().HasData(
            new WarehouseProductItem
            {
                Id = 1,
                ProductId = 1,
                ProductionFacilityId = 1,
                Quantity = 400,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseProductItem
            {
                Id = 2,
                ProductId = 2,
                ProductionFacilityId = 1,
                Quantity = 300,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseProductItem
            {
                Id = 3,
                ProductId = 1,
                ProductionFacilityId = 2,
                Quantity = 700,
                CreateTime = DateTime.UtcNow,
            },
            new WarehouseProductItem
            {
                Id = 4,
                ProductId = 2,
                ProductionFacilityId = 2,
                Quantity = 600,
                CreateTime = DateTime.UtcNow,
            });

            #endregion Seeding
        }

        /// <summary>
        /// Set update info on updated ICreateUpdateTime entity.
        /// </summary>
        private void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
        {
            EntityEntry entry = e.Entry;

            if (e.NewState != EntityState.Modified && e.NewState != EntityState.Deleted)
            {
                return;
            }

            if (entry.Entity is ICreateUpdateTime)
            {
                var entity = (ICreateUpdateTime)entry.Entity;
                entity.UpdateTime = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Set creation info on new ICreateUpdateTime entity.
        /// </summary>
        private void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
        {
            EntityEntry entry = e.Entry;
            if (e.FromQuery || entry.State != EntityState.Added)
            {
                return;
            }

            if (entry.Entity is ICreateUpdateTime)
            {
                var entity = (ICreateUpdateTime)entry.Entity;
                entity.CreateTime = DateTime.UtcNow;
            }
        }
    }
}
