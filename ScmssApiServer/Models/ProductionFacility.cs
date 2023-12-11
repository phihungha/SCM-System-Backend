using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class ProductionFacility : ISoftDeletable
    {
        public DateTime CreateTime { get; set; }
        public string? Description { get; set; }
        public required string Email { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public required string Location { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }

        public ICollection<ProductionOrder> ProductionOrder { get; set; }
            = new List<ProductionOrder>();

        public DateTime? UpdateTime { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();

        public ICollection<WarehouseProductItem> WarehouseProductItems { get; set; }
            = new List<WarehouseProductItem>();

        public ICollection<Product> WarehouseProducts { get; set; } = new List<Product>();

        public ICollection<Supply> WarehouseSupplies { get; set; } = new List<Supply>();

        public ICollection<WarehouseSupplyItem> WarehouseSupplyItems { get; set; }
                                                    = new List<WarehouseSupplyItem>();
    }

    public class ProductionFacilityMP : Profile
    {
        public ProductionFacilityMP()
        {
            CreateMap<ProductionFacility, ProductionFacilityDto>();
            CreateMap<ProductionFacilityInputDto, ProductionFacility>();
        }
    }
}
