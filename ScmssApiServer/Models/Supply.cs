namespace ScmssApiServer.Models
{
    public class Supply : Goods
    {
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; } = null!;

        public IList<PurchaseOrderItem> PurchaseOrderItems { get; set; }
            = new List<PurchaseOrderItem>();
    }
}
