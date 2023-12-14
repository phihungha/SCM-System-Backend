namespace ScmssApiServer.Models
{
    /// <summary>
    /// Interface for models that have a lifecycle (begin and end).
    /// </summary>
    public interface ILifecycle : ICreateUpdateTime
    {
        User CreateUser { get; set; }
        string CreateUserId { get; set; }
        DateTime? EndTime { get; }
        User? EndUser { get; }
        string? EndUserId { get; }
        bool IsEnded => EndTime != null;
        string? Problem { get; }

        void Begin(User user);

        void End(User user);

        void EndWithProblem(User user, string problem);
    }
}
