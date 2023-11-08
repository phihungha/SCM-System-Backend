namespace ScmssApiServer.Models
{
    public class PurchaseOrder : Order
    {
        public IList<PurchaseOrderItem> Items { get; set; }
            = new List<PurchaseOrderItem>();

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; } = null!;
        public required string Location { get; set; }

        public int PurchaseRequisitionId { get; set; }
        public PurchaseRequisition PurchaseRequisition { get; set; } = null!;

        public decimal DiscountAmount { get; set; }
    }
}
