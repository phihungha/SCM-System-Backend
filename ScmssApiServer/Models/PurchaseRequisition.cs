namespace ScmssApiServer.Models
{
    public class PurchaseRequisition : IUpdateTrackable
    {
        public int Id { get; set; }

        public IList<PurchaseRequisitionItem> Items { get; set; }
            = new List<PurchaseRequisitionItem>();

        public decimal SubTotal { get; set; }
        public double VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public PurchaseRequisitionStatus Status { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; } = null!;

        public int? PurchaseOrderId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }

        public int CreatedUserId { get; set; }
        public User CreatedUser { get; set; } = null!;
        public int? ApprovedProductionManagerId { get; set; }
        public User? ApprovedProductionManager { get; set; }
        public int? ApprovedFinanceId { get; set; }
        public User? ApprovedFinance { get; set; }
        public int? FinishedUserId { get; set; }
        public User? FinishedUser { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime? FinishedTime { get; set; }
    }
}
