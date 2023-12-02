using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public abstract class OrderDto<TItem, TEvent>
        where TItem : OrderItemDto
        where TEvent : OrderEventDto
    {
        public DateTime CreateTime { get; set; }

        public UserDto CreateUser { get; set; } = null!;

        public required string CreateUserId { get; set; }

        public ICollection<TEvent> Events { get; set; } = new List<TEvent>();

        public DateTime? ExecutionFinishTime { get; set; }

        public DateTime? FinishTime { get; set; }

        public UserDto? FinishUser { get; set; }

        public string? FinishUserId { get; set; }

        public int Id { get; set; }

        public ICollection<TItem> Items { get; set; } = new List<TItem>();

        public string? Problem { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
