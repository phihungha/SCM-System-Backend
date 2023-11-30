namespace ScmssApiServer.DTOs
{
    public abstract class OrderUpdateDto
    {
        public string? Problem { get; set; }
        public OrderStatusSelection? Status { get; set; }
    }
}
