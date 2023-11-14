namespace ScmssApiServer.Models
{
    public class ProductionOrderItem
    {
        public int ProductionOrderId { get; set; }
        public ProductionOrder ProductionOrder { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public double Quantity { get; set; }
        public required string Unit { get; set; }
        public decimal UnitValue { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
        public decimal TotalCost { get; set; }
    }
}
