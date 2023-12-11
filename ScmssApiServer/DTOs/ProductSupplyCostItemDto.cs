namespace ScmssApiServer.DTOs
{
    public class ProductSupplyCostItemDto
    {
        public double Quantity { get; set; }
        public SupplyDto Supply { get; set; } = null!;
        public int SupplyId { get; set; }
        public decimal TotalCost { get; set; }
        public required string Unit { get; set; }
        public decimal UnitCost { get; set; }
    }
}
