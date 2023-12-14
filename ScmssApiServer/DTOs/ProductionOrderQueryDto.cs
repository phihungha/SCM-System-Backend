using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class ProductionOrderQueryDto :
        OrderQueryDto<ProductionOrderSearchCriteria>,
        IApprovalStatusQueryDto
    {
        public ICollection<ApprovalStatus>? ApprovalStatus { get; set; }
    }
}
