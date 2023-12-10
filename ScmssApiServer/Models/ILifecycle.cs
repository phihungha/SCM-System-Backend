namespace ScmssApiServer.Models
{
    /// <summary>
    /// Interface for models that have a lifecycle (begin and end).
    /// </summary>
    public interface ILifecycle
    {
        DateTime CreateTime { get; set; }
        User CreateUser { get; set; }
        string CreateUserId { get; set; }
        DateTime? EndTime { get; }
        User? EndUser { get; }
        string? EndUserId { get; }
        bool IsEnded => EndTime != null;
        string? Problem { get; }
        DateTime? UpdateTime { get; set; }

        void Begin(string userId);

        void End(string userId);

        void EndWithProblem(string userId, string problem);
    }
}
