using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class ProductionOrderItemDto : OrderItemDto
    {
        public GoodsDto Product { get; set; } = null!;
        public decimal TotalCost { get; set; }
        public decimal TotalValue { get; set; }
        public decimal UnitCost { get; set; }
        public decimal UnitValue { get; set; }
    }
}
