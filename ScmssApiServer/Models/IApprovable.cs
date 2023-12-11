namespace ScmssApiServer.Models
{
    /// <summary>
    /// Interface for models that have approval ability.
    /// </summary>
    public interface IApprovable
    {
        ApprovalStatus ApprovalStatus { get; }

        void Approve(string userId);

        void Reject(string userId, string problem);
    }
}
