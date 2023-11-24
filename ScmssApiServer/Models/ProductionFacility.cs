using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class ProductionFacility : IUpdateTrackable
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Location { get; set; }

        public ICollection<ProductionOrder> ProductionOrder { get; set; }
            = new List<ProductionOrder>();

        public ICollection<WarehouseSupplyItem> WarehouseSupplyItems { get; set; }
            = new List<WarehouseSupplyItem>();

        public ICollection<Supply> WarehouseSupplies { get; set; } = new List<Supply>();

        public ICollection<WarehouseProductItem> WarehouseProductItems { get; set; }
            = new List<WarehouseProductItem>();

        public ICollection<Product> WarehouseProducts { get; set; } = new List<Product>();

        public ICollection<User> Users { get; set; } = new List<User>();

        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    public class ProductionFacilityMP : Profile
    {
        public ProductionFacilityMP()
        {
            CreateMap<ProductionFacility, ProductionFacilityDto>();
        }
    }
}
