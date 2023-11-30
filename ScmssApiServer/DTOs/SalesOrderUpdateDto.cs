namespace ScmssApiServer.DTOs
{
    public class SalesOrderUpdateDto
    {
        public IList<OrderItemInputDto>? Items { get; set; }
        public decimal? PaymentAmount { get; set; }
        public bool? PaymentCompleted { get; set; }
        public string? Problem { get; set; }
        public int? ProductionFacilityId { get; set; }
        public OrderStatusSelection? Status { get; set; }
        public string? ToLocation { get; set; }
    }
}
