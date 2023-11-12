using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ScmssApiServer.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    public class User : IdentityUser, IUpdateTrackable
    {
        [PersonalData]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public required string Name { get; set; }

        [PersonalData]
        public required Gender Gender { get; set; }

        [PersonalData]
        public required DateTime DateOfBirth { get; set; }

        [PersonalData]
        [StringLength(maximumLength: 12, MinimumLength = 12)]
        [Column(TypeName = "char(12)")]
        public string? IdCardNumber { get; set; }

        [PersonalData]
        public string? Address { get; set; }

        public string? Description { get; set; }

        public ICollection<PurchaseRequisition> CreatedPurchaseRequisitions { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<PurchaseRequisition> ApprovedPurchaseRequisitionsAsManager { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<PurchaseRequisition> ApprovedPurchaseRequisitionsAsFinance { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<PurchaseRequisition> FinishedPurchaseRequisitions { get; set; }
            = new List<PurchaseRequisition>();

        public ICollection<PurchaseOrder> CreatedPurchaseOrders { get; set; }
            = new List<PurchaseOrder>();

        public ICollection<PurchaseOrder> FinishedPurchaseOrders { get; set; }
            = new List<PurchaseOrder>();

        public ICollection<SalesOrder> CreatedSalesOrders { get; set; }
            = new List<SalesOrder>();

        public ICollection<SalesOrder> FinishedSalesOrders { get; set; }
            = new List<SalesOrder>();

        public ICollection<ProductionOrder> CreatedProductionOrders { get; set; }
            = new List<ProductionOrder>();

        public ICollection<ProductionOrder> ApprovedProductionOrdersAsManager { get; set; }
            = new List<ProductionOrder>();

        public ICollection<ProductionOrder> FinishedProductionOrders { get; set; }
            = new List<ProductionOrder>();

        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserInputDto, User>();
            CreateMap<UserCreateDto, User>();
        }
    }
}
