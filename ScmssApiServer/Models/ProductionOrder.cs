namespace ScmssApiServer.Models
{
    public class ProductionOrder : Order<
            ProductionOrderItem,
            ProductionOrderEvent>
    {
        public User? ApproveProductionManager { get; set; }
        public string? ApproveProductionManagerId { get; set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;
        public int ProductionFacilityId { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal TotalValue { get; set; }
    }
}
