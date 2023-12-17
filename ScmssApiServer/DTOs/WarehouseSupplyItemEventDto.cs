namespace ScmssApiServer.DTOs
{
    public class WarehouseSupplyItemEventDto : WarehouseItemEventDto
    {
        public int? ProductionOrderId { get; set; }
        public int? PurchaseOrderId { get; set; }
    }
}
