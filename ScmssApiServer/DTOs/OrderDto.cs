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
        public bool IsAcceptAllowed { get; set; }
        public bool IsCancelAllowed { get; set; }
        public bool IsExecuting { get; set; }
        public bool IsExecutionFinishAllowed { get; set; }
        public bool IsExecutionFinished { get; set; }
        public bool IsExecutionInfoUpdateAllowed { get; set; }
        public bool IsExecutionStartAllowed { get; set; }
        public bool IsProcessing { get; set; }
        public required ICollection<TItemDto> Items { get; set; }
        public OrderStatus Status { get; set; }
    }
}
