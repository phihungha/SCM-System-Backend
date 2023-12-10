namespace ScmssApiServer.DTOs
{
    public class ProductionOrderUpdateDto : OrderUpdateDto<OrderItemInputDto>
    {
        public ApprovalStatusOption? ApprovalStatus { get; set; }
    }
}
