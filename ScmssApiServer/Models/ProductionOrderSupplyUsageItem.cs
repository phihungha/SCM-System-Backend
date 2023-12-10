using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class ProductionOrderSupplyUsageItem
    {
        public ProductionOrder ProductionOrder { get; set; } = null!;
        public int ProductionOrderId { get; set; }
        public double Quantity { get; set; }
        public Supply Supply { get; set; } = null!;
        public int SupplyId { get; set; }

        public decimal TotalCost
        {
            get => UnitCost * (decimal)Quantity;
            private set => _ = value;
        }

        public required string Unit { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class ProductionOrderSupplyUsageItemMp : Profile
    {
        public ProductionOrderSupplyUsageItemMp()
        {
            CreateMap<ProductionOrderSupplyUsageItem, ProductionOrderSupplyUsageItemDto>();
        }
    }
}
