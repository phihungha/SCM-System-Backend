namespace ScmssApiServer.Models
{
    public class PurchaseOrderItem : TransOrderItem
    {
        public decimal Discount { get; set; }

        public decimal NetPrice
        {
            get => TotalPrice - Discount;
            private set => _ = value;
        }

        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        public Supply Supply { get; set; } = null!;
    }
}
