namespace ScmssApiServer.DTOs
{
    public class SalesOrderUpdateDto : SalesOrderInputDto
    {
        public IList<OrderItemInputDto>? Items { get; set; }

        public OrderStatusSelection? Status { get; set; }
        public bool? PaymentCompleted { get; set; }
    }
}
