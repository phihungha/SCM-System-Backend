namespace ScmssApiServer.Models
{
    public class WarehouseSupplyItemEvent : WarehouseItemEvent
    {
        public ProductionOrder? ProductionOrder { get; set; }
        public int? ProductionOrderId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
        public int? PurchaseOrderId { get; set; }
        public WarehouseSupplyItem WarehouseSupplyItem { get; set; } = null!;
        public int WarehouseSupplyItemId { get; set; }
    }
}
