namespace ScmssApiServer.Models
{
    public class ProductionOrderProgressUpdate : OrderProgressUpdate
    {
        public int ProductionOrderId { get; set; }
        public ProductionOrder ProductionOrder { get; set; } = null!;
    }
}
