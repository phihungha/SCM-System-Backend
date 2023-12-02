using ScmssApiServer.DomainExceptions;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// Abstract class for a model with standard lifecycle logic.
    /// </summary>
    public abstract class StandardLifecycle : ILifecycle
    {
        public DateTime CreateTime { get; set; }
        public User CreateUser { get; set; } = null!;
        public required string CreateUserId { get; set; }
        public DateTime? EndTime { get; private set; }
        public User? EndUser { get; private set; }
        public string? EndUserId { get; private set; }
        public abstract bool IsCreated { get; }
        public bool IsEnded { get => EndTime != null; }
        public string? Problem { get; private set; }
        public DateTime? UpdateTime { get; set; }

        public virtual void Begin(string userId)
        {
            if (IsCreated)
            {
                throw new InvalidDomainOperationException("Cannot begin an already created item.");
            }
            CreateUserId = userId;
        }

        public virtual void End(string userId)
        {
            if (IsEnded)
            {
                throw new InvalidDomainOperationException("Item is already ended.");
            }
            EndTime = DateTime.UtcNow;
            EndUserId = userId;
            CreateUserId = userId;
        }

        public void EndWithProblem(string userId, string problem)
        {
            Problem = problem;
            End(userId);
        }
    }
}
