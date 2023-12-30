namespace ScmssApiServer.DTOs
{
    public class HomeDto
    {
        public int ActiveProductionOrderCount { get; set; }
        public required ICollection<ProductionOrderDto> ActiveProductionOrders { get; set; }
        public int ActivePurchaseOrderCount { get; set; }
        public required ICollection<PurchaseOrderDto> ActivePurchaseOrders { get; set; }
        public int ActiveSalesOrderCount { get; set; }
        public required ICollection<SalesOrderDto> ActiveSalesOrders { get; set; }
    }
}
