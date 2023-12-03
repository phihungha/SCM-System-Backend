namespace ScmssApiServer.DTOs
{
    public abstract class StandardLifecycleDto
    {
        public DateTime CreateTime { get; set; }
        public required UserDto CreateUser { get; set; }
        public required string CreateUserId { get; set; }
        public DateTime? EndTime { get; set; }
        public UserDto? EndUser { get; set; }
        public string? EndUserId { get; set; }
        public DateTime? ExecutionFinishTime { get; set; }
        public string? Problem { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
