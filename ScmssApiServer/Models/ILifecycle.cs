namespace ScmssApiServer.Models
{
    public interface ILifecycle
    {
        public string CreateUserId { get; set; }
        public User CreateUser { get; set; }
        public string? FinishUserId { get; set; }
        public User? FinishUser { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? FinishTime { get; set; }
    }
}
