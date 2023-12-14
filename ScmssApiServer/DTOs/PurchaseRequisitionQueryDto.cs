using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class PurchaseRequisitionQueryDto : IApprovalStatusQueryDto
    {
        public ICollection<ApprovalStatus>? ApprovalStatus { get; set; }
        public PurchaseRequisitionSearchCriteria? SearchCriteria { get; set; }
        public string? SearchTerm { get; set; }
        public ICollection<PurchaseRequisitionStatus>? Status { get; set; }
    }
}
