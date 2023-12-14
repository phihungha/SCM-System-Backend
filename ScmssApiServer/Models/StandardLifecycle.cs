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
        public DateTime? EndTime { get; protected set; }
        public User? EndUser { get; protected set; }
        public string? EndUserId { get; protected set; }
        public abstract bool IsCreated { get; }
        public bool IsEnded => EndTime != null;
        public string? Problem { get; protected set; }
        public DateTime? UpdateTime { get; set; }

        public virtual void Begin(User user)
        {
            if (IsCreated)
            {
                throw new InvalidDomainOperationException("Cannot begin an already created item.");
            }
            CreateUser = user;
            CreateUserId = user.Id;
        }

        public virtual void End(User user)
        {
            if (IsEnded)
            {
                throw new InvalidDomainOperationException("The item is already ended.");
            }
            EndTime = DateTime.UtcNow;
            EndUser = user;
            EndUserId = user.Id;
        }

        public void EndWithProblem(User user, string problem)
        {
            Problem = problem;
            End(user);
        }
    }
}
