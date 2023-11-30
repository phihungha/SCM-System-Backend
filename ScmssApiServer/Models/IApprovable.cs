namespace ScmssApiServer.Models
{
    public interface IApprovable
    {
        public void Approve(string userId);

        public void Reject(string userId, string problem);

        public ApprovalStatus ApprovalStatus { get; }
    }
}
