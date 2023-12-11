namespace ScmssApiServer.DTOs
{
    public class ProductionOrderSupplyUsageItemDto
    {
        public double Quantity { get; set; }
        public required SupplyDto Supply { get; set; }
        public int SupplyId { get; set; }
        public decimal TotalCost { get; set; }
        public required string Unit { get; set; }
        public decimal UnitCost { get; set; }
    }
}
