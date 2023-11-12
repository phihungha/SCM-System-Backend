namespace ScmssApiServer.Models
{
    public class ProductionOrder : ILifecycle
    {
        public int Id { get; set; }

        public ICollection<ProductionOrderItem> ProductionOrderItems { get; set; }
            = new List<ProductionOrderItem>();

        public ICollection<Product> Products { get; set; }
            = new List<Product>();

        public decimal TotalValue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }

        public ProductionOrderStatus Status { get; set; }

        public int ProductionFacilityId { get; set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;

        public required string CreateUserId { get; set; }
        public User CreateUser { get; set; } = null!;
        public string? ApproveProductionManagerId { get; set; }
        public User? ApproveProductionManager { get; set; }
        public string? FinishUserId { get; set; }
        public User? FinishUser { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
    }
}
