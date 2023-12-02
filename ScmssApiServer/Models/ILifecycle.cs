namespace ScmssApiServer.Models
{
    public interface ILifecycle
    {
        public DateTime CreateTime { get; set; }
        public User CreateUser { get; set; }
        public string CreateUserId { get; set; }
        public DateTime? EndTime { get; }
        public User? EndUser { get; }
        public string? EndUserId { get; }
        public bool IsEnded { get => EndTime != null; }
        public DateTime? UpdateTime { get; set; }

        public void Begin(string userId);

        public void End(string userId);
    }
}
