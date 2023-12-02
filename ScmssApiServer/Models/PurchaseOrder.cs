namespace ScmssApiServer.Models
{
    public class PurchaseOrder : TransOrder<PurchaseOrderItem, PurchaseOrderEvent>
    {
        public decimal DiscountAmount { get; set; }
        public PurchaseRequisition? PurchaseRequisition { get; set; }
        public int? PurchaseRequisitionId { get; set; }
        public ICollection<Supply> Supplies { get; set; } = new List<Supply>();
        public Vendor Vendor { get; set; } = null!;
        public int VendorId { get; set; }
    }
}
