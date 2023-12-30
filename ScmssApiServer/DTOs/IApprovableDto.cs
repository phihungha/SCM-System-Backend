using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public interface IApprovableDto
    {
        ApprovalStatus ApprovalStatus { get; set; }
        public bool IsApprovalAllowed { get; set; }
    }
}
