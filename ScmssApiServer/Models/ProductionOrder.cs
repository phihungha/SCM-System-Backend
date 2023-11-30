namespace ScmssApiServer.Models
{
    public class ProductionOrder : Order<
            ProductionOrderItem,
            ProductionOrderEvent>
    {
        public ICollection<Product> Products { get; set; }
            = new List<Product>();

        public decimal TotalValue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }

        public ProductionOrderStatus Status { get; set; }

        public int ProductionFacilityId { get; set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;

        public string? ApproveProductionManagerId { get; set; }
        public User? ApproveProductionManager { get; set; }

        public override void Start(string userId)
        {
            base.Start(userId);
            Status = ProductionOrderStatus.PendingApproval;
        }
    }
}
