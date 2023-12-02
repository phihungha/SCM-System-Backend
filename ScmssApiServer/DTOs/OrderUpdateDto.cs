namespace ScmssApiServer.DTOs
{
    public abstract class OrderUpdateDto
    {
        public string? Problem { get; set; }
        public OrderStatusSelection? Status { get; set; }
        public IList<OrderItemInputDto>? Items { get; set; }
    }
}
