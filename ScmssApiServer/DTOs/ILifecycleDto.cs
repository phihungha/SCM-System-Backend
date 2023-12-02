using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public interface ILifecycle
    {
        public DateTime CreateTime { get; set; }
        public UserDto CreateUser { get; set; }
        public string CreateUserId { get; set; }
        public DateTime? EndTime { get; }
        public UserDto? EndUser { get; }
        public string? EndUserId { get; }
        public DateTime? UpdateTime { get; set; }
    }
}
