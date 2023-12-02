namespace ScmssApiServer.Models
{
    public class PurchaseOrderItem : TransOrderItem
    {
        public decimal Discount { get; set; }
        public decimal NetPrice { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        public Supply Supply { get; set; } = null!;
    }
}
