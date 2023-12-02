using ScmssApiServer.DomainExceptions;

namespace ScmssApiServer.Models
{
    public abstract class ApprovableLifecycle : StandardLifecycle, IApprovable
    {
        public ApprovalStatus ApprovalStatus { get; private set; }

        public virtual void Approve(string userId)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot approve item which isn't currently waiting for approval."
                    );
            }
            ApprovalStatus = ApprovalStatus.Approved;
        }

        public override void Begin(string userId)
        {
            base.Begin(userId);
            ApprovalStatus = ApprovalStatus.PendingApproval;
        }

        public virtual void Reject(string userId, string problem)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot reject item which isn't waiting for approval."
                    );
            }
            ApprovalStatus = ApprovalStatus.Rejected;
            EndWithProblem(userId, problem);
        }
    }
}
