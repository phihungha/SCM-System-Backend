using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public interface IApprovableDto
    {
        ApprovalStatus ApprovalStatus { get; set; }
    }
}
