namespace ScmssApiServer.Models
{
    public interface IApprovable
    {
        public void Approve(string userId);

        public void Reject(string userId);

        public ApprovalStatus ApprovalStatus { get; }
    }
}
