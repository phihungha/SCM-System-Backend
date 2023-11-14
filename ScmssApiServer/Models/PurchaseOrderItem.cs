namespace ScmssApiServer.Models
{
    public class PurchaseOrderItem : OrderItem
    {
        public Supply Supply { get; set; } = null!;
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public decimal Discount { get; set; }
        public decimal NetPrice { get; set; }
    }
}
