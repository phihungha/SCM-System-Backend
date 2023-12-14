using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public interface IApprovalStatusQueryDto
    {
        public ICollection<ApprovalStatus>? ApprovalStatus { get; set; }
    }
}
