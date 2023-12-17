namespace ScmssApiServer.DTOs
{
    public class WarehouseProductItemEventDto : WarehouseItemEventDto
    {
        public int? ProductionOrderId { get; set; }
        public int? SalesOrderId { get; set; }
    }
}
