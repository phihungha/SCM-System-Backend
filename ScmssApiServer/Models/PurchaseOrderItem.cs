namespace ScmssApiServer.Models
{
    public class PurchaseOrderItem : TransOrderItem
    {
        public Supply Supply { get; set; } = null!;
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public decimal Discount { get; set; }
        public decimal NetPrice { get; set; }
    }
}
