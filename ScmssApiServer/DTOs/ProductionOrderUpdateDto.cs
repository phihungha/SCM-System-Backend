using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class ProductionOrderUpdateDto : OrderUpdateDto
    {
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
