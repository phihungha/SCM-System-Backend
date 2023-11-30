using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ScmssApiServer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactPerson = table.Column<string>(type: "text", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DefaultLocation = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionFacilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionFacilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MiscCost = table.Column<decimal>(type: "numeric", nullable: false),
                    NetWeight = table.Column<double>(type: "double precision", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ExpirationMonth = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactPerson = table.Column<string>(type: "text", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DefaultLocation = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdCardNumber = table.Column<string>(type: "char(12)", maxLength: 12, nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProductionFacilityId = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_ProductionFacilities_ProductionFacilityId",
                        column: x => x.ProductionFacilityId,
                        principalTable: "ProductionFacilities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WarehouseProductItem",
                columns: table => new
                {
                    ProductionFacilityId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseProductItem", x => new { x.ProductId, x.ProductionFacilityId });
                    table.ForeignKey(
                        name: "FK_WarehouseProductItem_ProductionFacilities_ProductionFacilit~",
                        column: x => x.ProductionFacilityId,
                        principalTable: "ProductionFacilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseProductItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Supplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VendorId = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ExpirationMonth = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplies_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApproveProductionManagerId = table.Column<string>(type: "text", nullable: true),
                    ProductionFacilityId = table.Column<int>(type: "integer", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalProfit = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalValue = table.Column<decimal>(type: "numeric", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<string>(type: "text", nullable: false),
                    ExecutionFinishTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishUserId = table.Column<string>(type: "text", nullable: true),
                    Problem = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionOrders_AspNetUsers_ApproveProductionManagerId",
                        column: x => x.ApproveProductionManagerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_AspNetUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionOrders_AspNetUsers_FinishUserId",
                        column: x => x.FinishUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_ProductionFacilities_ProductionFacilityId",
                        column: x => x.ProductionFacilityId,
                        principalTable: "ProductionFacilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequisitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    VatRate = table.Column<double>(type: "double precision", nullable: false),
                    VatAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    VendorId = table.Column<int>(type: "integer", nullable: false),
                    ProductionFacilityId = table.Column<int>(type: "integer", nullable: false),
                    CreateUserId = table.Column<string>(type: "text", nullable: false),
                    ApproveProductionManagerId = table.Column<string>(type: "text", nullable: true),
                    ApproveFinanceId = table.Column<string>(type: "text", nullable: true),
                    FinishUserId = table.Column<string>(type: "text", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequisitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRequisitions_AspNetUsers_ApproveFinanceId",
                        column: x => x.ApproveFinanceId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequisitions_AspNetUsers_ApproveProductionManagerId",
                        column: x => x.ApproveProductionManagerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequisitions_AspNetUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRequisitions_AspNetUsers_FinishUserId",
                        column: x => x.FinishUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequisitions_ProductionFacilities_ProductionFacilit~",
                        column: x => x.ProductionFacilityId,
                        principalTable: "ProductionFacilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRequisitions_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<string>(type: "text", nullable: false),
                    ExecutionFinishTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishUserId = table.Column<string>(type: "text", nullable: true),
                    Problem = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FromLocation = table.Column<string>(type: "text", nullable: true),
                    InvoiceUrl = table.Column<string>(type: "text", nullable: true),
                    PaymentStatus = table.Column<string>(type: "text", nullable: false),
                    ProductionFacilityId = table.Column<int>(type: "integer", nullable: true),
                    ReceiptUrl = table.Column<string>(type: "text", nullable: true),
                    RemainingAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    SubTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    ToLocation = table.Column<string>(type: "text", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    VatAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    VatRate = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrders_AspNetUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesOrders_AspNetUsers_FinishUserId",
                        column: x => x.FinishUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesOrders_ProductionFacilities_ProductionFacilityId",
                        column: x => x.ProductionFacilityId,
                        principalTable: "ProductionFacilities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductionSupplyCostItem",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    SupplyId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionSupplyCostItem", x => new { x.ProductId, x.SupplyId });
                    table.ForeignKey(
                        name: "FK_ProductionSupplyCostItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionSupplyCostItem_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseSupplyItem",
                columns: table => new
                {
                    ProductionFacilityId = table.Column<int>(type: "integer", nullable: false),
                    SupplyId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseSupplyItem", x => new { x.ProductionFacilityId, x.SupplyId });
                    table.ForeignKey(
                        name: "FK_WarehouseSupplyItem_ProductionFacilities_ProductionFacility~",
                        column: x => x.ProductionFacilityId,
                        principalTable: "ProductionFacilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseSupplyItem_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionOrderEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductionOrderId = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOrderEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionOrderEvent_ProductionOrders_ProductionOrderId",
                        column: x => x.ProductionOrderId,
                        principalTable: "ProductionOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionOrderItem",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalValue = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitValue = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOrderItem", x => new { x.ItemId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_ProductionOrderItem_ProductionOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "ProductionOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionOrderItem_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscountAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    PurchaseRequisitionId = table.Column<int>(type: "integer", nullable: true),
                    VendorId = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<string>(type: "text", nullable: false),
                    ExecutionFinishTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishUserId = table.Column<string>(type: "text", nullable: true),
                    Problem = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FromLocation = table.Column<string>(type: "text", nullable: true),
                    InvoiceUrl = table.Column<string>(type: "text", nullable: true),
                    PaymentStatus = table.Column<string>(type: "text", nullable: false),
                    ProductionFacilityId = table.Column<int>(type: "integer", nullable: true),
                    ReceiptUrl = table.Column<string>(type: "text", nullable: true),
                    RemainingAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    SubTotal = table.Column<decimal>(type: "numeric", nullable: false),
                    ToLocation = table.Column<string>(type: "text", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    VatAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    VatRate = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_AspNetUsers_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_AspNetUsers_FinishUserId",
                        column: x => x.FinishUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_ProductionFacilities_ProductionFacilityId",
                        column: x => x.ProductionFacilityId,
                        principalTable: "ProductionFacilities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PurchaseRequisitions_PurchaseRequisitionId",
                        column: x => x.PurchaseRequisitionId,
                        principalTable: "PurchaseRequisitions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequisitionItem",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequisitionItem", x => new { x.ItemId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_PurchaseRequisitionItem_PurchaseRequisitions_OrderId",
                        column: x => x.OrderId,
                        principalTable: "PurchaseRequisitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRequisitionItem_Supplies_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SalesOrderId = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrderEvent_SalesOrders_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderItem",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderItem", x => new { x.ItemId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_SalesOrderItem_Products_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesOrderItem_SalesOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "SalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseOrderId = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderEvent_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderItem",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric", nullable: false),
                    NetPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderItem", x => new { x.ItemId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItem_PurchaseOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItem_Supplies_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "ContactPerson", "CreateTime", "DefaultLocation", "Description", "Email", "IsActive", "Name", "PhoneNumber", "UpdateTime" },
                values: new object[,]
                {
                    { 1, "Hoa Thi Mai", new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(6819), "156 Nguyen Van Luong, Bien Hoa, Dong nai", "Flower garden.", "watarichanno@gmail.com", true, "Cool Garden 324", "0344250401", null },
                    { 2, "Ha Phi Hung", new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(6826), "436 Vo Van Kiet, District 1, HCM City", "Plant shop.", "haphihung55@gmail.com", true, "Phi Hung Shop", "0344250401", null }
                });

            migrationBuilder.InsertData(
                table: "ProductionFacilities",
                columns: new[] { "Id", "CreateTime", "Description", "Email", "IsActive", "Location", "Name", "PhoneNumber", "UpdateTime" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7323), "Primary production facility", "godau@cool-fertilizer.com.vn", true, "Go Dau Industrial Park, Phuoc Thai, Long Thanh, Dong Nai", "Go Dau", "02837560110", null },
                    { 2, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7328), "Secondary production facility", "longan@cool-fertilizer.com.vn", true, "Long Dinh Industrial Park, Long Dinh, Can Duoc, Long An", "Binh Dien - Long An", "02723726627", null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreateTime", "Description", "ExpirationMonth", "IsActive", "MiscCost", "Name", "NetWeight", "Price", "Unit", "UpdateTime" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7527), "MSPB: 04513\nProtein total (Nts): 16%\nEffective Phosphate (P2O5hh): 8%\nEffective Potassium (K2Ohh): 8%\nSulfur (S): 13%\nHumidity: 2%\nSuitable for all crops.", 48, true, 15000m, "NPK 16-8-8+13S", 50.0, 500000m, "Item(s)", null },
                    { 2, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7532), "MSPB: 04519\nProtein total (Nts): 16%\nEffective Phosphate (P2O5hh): 7%\nEffective Potassium (K2Ohh): 18%\nSulfur (S): 12%\nBo (B): 217ppm\nZinc (Zn): 400ppm\nHumidity: 2%\nSuitable for coffee, fruit, rubber, vegetable, rice crops.", 48, true, 18000m, "NPK 16-7-18+12S+TE", 50.0, 600000m, "Item(s)", null }
                });

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "Id", "ContactPerson", "CreateTime", "DefaultLocation", "Description", "Email", "IsActive", "Name", "PhoneNumber", "UpdateTime" },
                values: new object[,]
                {
                    { 1, "Ha Long Anh", new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7377), "Phu My Industrial Park, Phu My, Phu My, Ba Ria - Vung Tau", "Main vendor for major ingredients.", "customer@pvfcco.com.vn", true, "PVFCCo", "02838256258", null },
                    { 2, "Nguyen Thanh Long", new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7380), "Binh Duong Industrial Park, An Binh, Di An, Binh Duong", "Main vendor for trace ingredients.", "order@vinachem.com.vn", true, "Vinachem", "02438240551", null }
                });

            migrationBuilder.InsertData(
                table: "Supplies",
                columns: new[] { "Id", "CreateTime", "Description", "ExpirationMonth", "IsActive", "Name", "Price", "Unit", "UpdateTime", "VendorId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7459), "CO(NH2)2 for nitrogen.", 12, true, "PVFCCo Urea", 5000m, "Kg", null, 1 },
                    { 2, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7463), "P2O5 for phosphorous.", 12, true, "PVFCCo Phosphorous", 6000m, "Kg", null, 1 },
                    { 3, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7467), "KCl for potassium.", 12, true, "PVFCCo Potassium Chloride", 5000m, "Kg", null, 1 },
                    { 4, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7471), "(NH4)2SO4 for trace sulfur.", 12, true, "Vinachem Ammonium Sulphate", 13000m, "Kg", null, 2 },
                    { 5, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7476), "H3BO3 for trace boron.", 12, true, "Vinachem Boric Acid", 38000m, "Kg", null, 2 },
                    { 6, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7483), "ZnSO4 for trace zinc.", 12, true, "Vinachem Zinc Sulphate", 40000m, "Kg", null, 2 }
                });

            migrationBuilder.InsertData(
                table: "WarehouseProductItem",
                columns: new[] { "ProductId", "ProductionFacilityId", "CreateTime", "IsActive", "Quantity", "UpdateTime" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7669), true, 400.0, null },
                    { 1, 2, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7672), true, 700.0, null },
                    { 2, 1, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7671), true, 300.0, null },
                    { 2, 2, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7682), true, 600.0, null }
                });

            migrationBuilder.InsertData(
                table: "ProductionSupplyCostItem",
                columns: new[] { "ProductId", "SupplyId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 20.600000000000001 },
                    { 1, 2, 8.0 },
                    { 1, 3, 8.0 },
                    { 1, 4, 13.0 },
                    { 2, 1, 16.0 },
                    { 2, 2, 8.0 },
                    { 2, 3, 13.0 },
                    { 2, 4, 10.0 },
                    { 2, 5, 1.5 },
                    { 2, 6, 1.5 }
                });

            migrationBuilder.InsertData(
                table: "WarehouseSupplyItem",
                columns: new[] { "ProductionFacilityId", "SupplyId", "CreateTime", "IsActive", "Quantity", "UpdateTime" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7616), true, 13000.0, null },
                    { 1, 2, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7618), true, 12500.0, null },
                    { 1, 3, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7620), true, 12500.0, null },
                    { 1, 4, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7622), true, 12000.0, null },
                    { 1, 5, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7625), true, 1800.0, null },
                    { 1, 6, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7626), true, 1800.0, null },
                    { 2, 1, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7627), true, 12000.0, null },
                    { 2, 2, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7629), true, 12000.0, null },
                    { 2, 3, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7630), true, 12500.0, null },
                    { 2, 4, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7632), true, 11000.0, null },
                    { 2, 5, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7634), true, 1500.0, null },
                    { 2, 6, new DateTime(2023, 11, 30, 15, 34, 26, 218, DateTimeKind.Utc).AddTicks(7635), true, 1500.0, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProductionFacilityId",
                table: "AspNetUsers",
                column: "ProductionFacilityId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrderEvent_ProductionOrderId",
                table: "ProductionOrderEvent",
                column: "ProductionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrderItem_OrderId",
                table: "ProductionOrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_ApproveProductionManagerId",
                table: "ProductionOrders",
                column: "ApproveProductionManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_CreateUserId",
                table: "ProductionOrders",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_FinishUserId",
                table: "ProductionOrders",
                column: "FinishUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_ProductionFacilityId",
                table: "ProductionOrders",
                column: "ProductionFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionSupplyCostItem_SupplyId",
                table: "ProductionSupplyCostItem",
                column: "SupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderEvent_PurchaseOrderId",
                table: "PurchaseOrderEvent",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItem_OrderId",
                table: "PurchaseOrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CreateUserId",
                table: "PurchaseOrders",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_FinishUserId",
                table: "PurchaseOrders",
                column: "FinishUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ProductionFacilityId",
                table: "PurchaseOrders",
                column: "ProductionFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseRequisitionId",
                table: "PurchaseOrders",
                column: "PurchaseRequisitionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_VendorId",
                table: "PurchaseOrders",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitionItem_OrderId",
                table: "PurchaseRequisitionItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitions_ApproveFinanceId",
                table: "PurchaseRequisitions",
                column: "ApproveFinanceId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitions_ApproveProductionManagerId",
                table: "PurchaseRequisitions",
                column: "ApproveProductionManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitions_CreateUserId",
                table: "PurchaseRequisitions",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitions_FinishUserId",
                table: "PurchaseRequisitions",
                column: "FinishUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitions_ProductionFacilityId",
                table: "PurchaseRequisitions",
                column: "ProductionFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequisitions_VendorId",
                table: "PurchaseRequisitions",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderEvent_SalesOrderId",
                table: "SalesOrderEvent",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderItem_OrderId",
                table: "SalesOrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CreateUserId",
                table: "SalesOrders",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CustomerId",
                table: "SalesOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_FinishUserId",
                table: "SalesOrders",
                column: "FinishUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_ProductionFacilityId",
                table: "SalesOrders",
                column: "ProductionFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_VendorId",
                table: "Supplies",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProductItem_ProductionFacilityId",
                table: "WarehouseProductItem",
                column: "ProductionFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSupplyItem_SupplyId",
                table: "WarehouseSupplyItem",
                column: "SupplyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ProductionOrderEvent");

            migrationBuilder.DropTable(
                name: "ProductionOrderItem");

            migrationBuilder.DropTable(
                name: "ProductionSupplyCostItem");

            migrationBuilder.DropTable(
                name: "PurchaseOrderEvent");

            migrationBuilder.DropTable(
                name: "PurchaseOrderItem");

            migrationBuilder.DropTable(
                name: "PurchaseRequisitionItem");

            migrationBuilder.DropTable(
                name: "SalesOrderEvent");

            migrationBuilder.DropTable(
                name: "SalesOrderItem");

            migrationBuilder.DropTable(
                name: "WarehouseProductItem");

            migrationBuilder.DropTable(
                name: "WarehouseSupplyItem");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ProductionOrders");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "SalesOrders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.DropTable(
                name: "PurchaseRequisitions");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "ProductionFacilities");
        }
    }
}
