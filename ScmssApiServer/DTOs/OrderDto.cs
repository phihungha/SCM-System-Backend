using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public abstract class OrderDto
    {
        public DateTime CreateTime { get; set; }
        public UserDto CreateUser { get; set; } = null!;
        public required string CreateUserId { get; set; }
        public DateTime? ExecutionFinishTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public UserDto? FinishUser { get; set; }
        public string? FinishUserId { get; set; }
        public int Id { get; set; }
        public string? Problem { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
