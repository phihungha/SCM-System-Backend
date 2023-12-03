namespace ScmssApiServer.DTOs
{
    public abstract class OrderUpdateDto<TItemDto>
    {
        public string? Problem { get; set; }
        public OrderStatusOption? Status { get; set; }
        public ICollection<TItemDto>? Items { get; set; }
    }
}
