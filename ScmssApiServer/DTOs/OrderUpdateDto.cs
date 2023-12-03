namespace ScmssApiServer.DTOs
{
    public abstract class OrderUpdateDto<TItemDto>
    {
        public string? Problem { get; set; }
        public OrderStatusSelection? Status { get; set; }
        public ICollection<TItemDto>? Items { get; set; }
    }
}
