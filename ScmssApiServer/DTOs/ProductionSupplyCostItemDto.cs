namespace ScmssApiServer.DTOs
{
    public class ProductionSupplyCostItemDto
    {
        public int SupplyId { get; set; }
        public SupplyDto Supply { get; set; } = null!;
        public double Quantity { get; set; }
        public required string Unit { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
    }
}
