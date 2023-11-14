namespace ScmssApiServer.Models
{
    public class PurchaseRequisitionItem : OrderItem
    {
        public Supply Supply { get; set; } = null!;
        public PurchaseRequisition PurchaseRequisition { get; set; } = null!;
    }
}
