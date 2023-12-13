using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ScmssApiServer.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class User : IdentityUser, ISoftDeletable
    {
        [PersonalData]
        public string? Address { get; set; }

        public ICollection<ProductionOrder> ApprovedProductionOrdersAsManager { get; set; }
            = new List<ProductionOrder>();

        public ICollection<PurchaseRequisition> ApprovedPurchaseRequisitionsAsFinance { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<PurchaseRequisition> ApprovedPurchaseRequisitionsAsManager { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<ProductionOrder> CreatedProductionOrders { get; set; }
            = new List<ProductionOrder>();

        public ICollection<PurchaseOrder> CreatedPurchaseOrders { get; set; }
            = new List<PurchaseOrder>();

        public ICollection<PurchaseRequisition> CreatedPurchaseRequisitions { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<SalesOrder> CreatedSalesOrders { get; set; }
            = new List<SalesOrder>();

        public DateTime CreateTime { get; set; }

        [PersonalData]
        public required DateTime DateOfBirth { get; set; }

        public string? Description { get; set; }

        public ICollection<ProductionOrder> FinishedProductionOrders { get; set; }
            = new List<ProductionOrder>();

        public ICollection<PurchaseOrder> FinishedPurchaseOrders { get; set; }
            = new List<PurchaseOrder>();

        public ICollection<PurchaseRequisition> FinishedPurchaseRequisitions { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<SalesOrder> FinishedSalesOrders { get; set; }
            = new List<SalesOrder>();

        [PersonalData]
        public required Gender Gender { get; set; }

        [PersonalData]
        [StringLength(maximumLength: 12, MinimumLength = 12)]
        [Column(TypeName = "char(12)")]
        public string? IdCardNumber { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public bool IsFinance => Roles.Contains("Finance");

        [NotMapped]
        public bool IsInventoryUser => Roles.Contains("InventorySpecialist") ||
                                       Roles.Contains("InventoryManager");

        [NotMapped]
        public bool IsProductionUser => Roles.Contains("ProductionManager") ||
                                        Roles.Contains("ProductionSpecialist");

        [NotMapped]
        public bool IsPurchaseUser => Roles.Contains("PurchaseManager") ||
                                      Roles.Contains("PurchaseSpecialist");

        [NotMapped]
        public bool IsSales => Roles.Contains("SalesManager") ||
                               Roles.Contains("SalesSpecialist");

        [PersonalData]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public required string Name { get; set; }

        public ProductionFacility? ProductionFacility { get; set; }

        public int? ProductionFacilityId { get; set; }

        [NotMapped]
        public IList<string> Roles { get; set; } = new List<string>();

        public DateTime? UpdateTime { get; set; }
    }

    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>().ForMember(
                i => i.DateOfBirth,
                o => o.MapFrom(i => i.DateOfBirth.ToString(
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture)));

            CreateMap<UserInputDto, User>().ForMember(
                i => i.DateOfBirth,
                o => o.MapFrom(
                    i => DateOnly.Parse(i.DateOfBirth)
                                 .ToDateTime(new TimeOnly(), DateTimeKind.Utc)));

            CreateMap<UserCreateDto, User>().ForMember(
                i => i.DateOfBirth,
                o => o.MapFrom(
                    i => DateOnly.Parse(i.DateOfBirth)
                                 .ToDateTime(new TimeOnly(), DateTimeKind.Utc)));
        }
    }
}
