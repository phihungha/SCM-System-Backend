using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class ProductionOrderUpdateDto : OrderUpdateDto<OrderItemInputDto>
    {
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
