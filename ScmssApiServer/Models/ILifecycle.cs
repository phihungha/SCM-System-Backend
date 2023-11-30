namespace ScmssApiServer.Models
{
    public interface ILifecycle
    {
        public DateTime CreateTime { get; set; }
        public User CreateUser { get; set; }
        public string CreateUserId { get; set; }
        public bool IsFinished { get => FinishTime != null; }
        public DateTime? FinishTime { get; }
        public User? FinishUser { get; }
        public string? FinishUserId { get; }
        public DateTime? UpdateTime { get; set; }

        public void Finish(string userId);

        public void Begin(string userId);
    }
}
