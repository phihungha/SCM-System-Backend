namespace ScmssApiServer.DTOs
{
    public class ProductionOrderUpdateDto : OrderUpdateDto<OrderItemInputDto>
    {
        public ApprovalStatusSelection ApprovalStatus { get; set; }
    }
}
