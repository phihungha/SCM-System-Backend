using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public abstract class OrderDto<TItemDto, TEventDto> : StandardLifecycleDto
        where TItemDto : OrderItemDto
        where TEventDto : OrderEventDto
    {
        public required ICollection<TEventDto> Events { get; set; }
        public DateTime? ExecutionFinishTime { get; set; }
        public int Id { get; set; }
        public required ICollection<TItemDto> Items { get; set; }
        public OrderStatus Status { get; set; }
    }
}
