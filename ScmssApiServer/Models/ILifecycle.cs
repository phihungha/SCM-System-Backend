namespace ScmssApiServer.Models
{
    public interface ILifecycle
    {
        public string CreateUserId { get; set; }
        public User CreateUser { get; set; }
        public string? FinishUserId { get; }
        public User? FinishUser { get; }

        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? FinishTime { get; }

        public void Start(string userId);
        public void Finish(string userId);
    }
}
