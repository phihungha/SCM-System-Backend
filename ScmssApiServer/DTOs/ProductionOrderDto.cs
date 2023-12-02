using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class ProductionOrderDto : OrderDto<ProductionOrderItemDto, ProductionOrderEventDto>
    {
        public ApprovalStatus ApprovalStatus { get; set; }
        public UserDto? ApproveProductionManager { get; set; }
        public string? ApproveProductionManagerId { get; set; }
        public ProductionFacilityDto ProductionFacility { get; set; } = null!;
        public int ProductionFacilityId { get; set; }
        public ICollection<GoodsDto> Products { get; set; } = new List<GoodsDto>();
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal TotalValue { get; set; }
    }
}
