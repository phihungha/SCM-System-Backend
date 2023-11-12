namespace ScmssApiServer.Models
{
    public class PurchaseOrder : Order
    {
        public ICollection<PurchaseOrderItem> Items { get; set; }
            = new List<PurchaseOrderItem>();

        public ICollection<Supply> Supplies { get; set; } = new List<Supply>();

        public decimal DiscountAmount { get; set; }

        public ICollection<PurchaseOrderProgressUpdate> ProgressUpdates { get; set; }
            = new List<PurchaseOrderProgressUpdate>();

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; } = null!;
        public int ProductionFacilityId { get; set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;

        public int? PurchaseRequisitionId { get; set; }
        public PurchaseRequisition? PurchaseRequisition { get; set; }

    }
}
