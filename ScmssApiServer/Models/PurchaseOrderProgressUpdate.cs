namespace ScmssApiServer.Models
{
    public class PurchaseOrderProgressUpdate : OrderProgressUpdate
    {
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
    }
}
