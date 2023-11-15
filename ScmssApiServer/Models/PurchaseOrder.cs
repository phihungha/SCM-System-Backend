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

        public override required string? FromLocation { get; set; }

        public override string ToLocation
        {
            get => ProductionFacility.Location;
            set => _ = value;
        }

        public int? PurchaseRequisitionId { get; set; }
        public PurchaseRequisition? PurchaseRequisition { get; set; }
    }
}
