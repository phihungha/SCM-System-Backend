namespace ScmssApiServer.Models
{
    public class PurchaseRequisition : ILifecycle
    {
        public int Id { get; set; }

        public ICollection<PurchaseRequisitionItem> Items { get; set; }
            = new List<PurchaseRequisitionItem>();

        public ICollection<Supply> Supplies { get; set; } = new List<Supply>();

        public decimal SubTotal { get; set; }
        public double VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public PurchaseRequisitionStatus Status { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; } = null!;
        public int ProductionFacilityId { get; set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;

        public PurchaseOrder? PurchaseOrder { get; set; }

        public required string CreateUserId { get; set; }
        public User CreateUser { get; set; } = null!;
        public string? ApproveProductionManagerId { get; set; }
        public User? ApproveProductionManager { get; set; }
        public string? ApproveFinanceId { get; set; }
        public User? ApproveFinance { get; set; }
        public string? FinishUserId { get; set; }
        public User? FinishUser { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? FinishTime { get; set; }

        public void Finish(string userId)
        {
            throw new NotImplementedException();
        }

        public void Start(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
