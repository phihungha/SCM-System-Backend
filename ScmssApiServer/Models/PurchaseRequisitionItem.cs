namespace ScmssApiServer.Models
{
    public class PurchaseRequisitionItem : TransOrderItem
    {
        public Supply Supply { get; set; } = null!;
        public PurchaseRequisition PurchaseRequisition { get; set; } = null!;
    }
}
