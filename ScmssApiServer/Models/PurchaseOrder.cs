namespace ScmssApiServer.Models
{
    public class PurchaseOrder : Order<PurchaseOrderItem, PurchaseOrderEvent>
    {
        public ICollection<Supply> Supplies { get; set; } = new List<Supply>();

        public decimal DiscountAmount { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; } = null!;
        public int ProductionFacilityId { get; set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;

        public int? PurchaseRequisitionId { get; set; }
        public PurchaseRequisition? PurchaseRequisition { get; set; }
    }
}
