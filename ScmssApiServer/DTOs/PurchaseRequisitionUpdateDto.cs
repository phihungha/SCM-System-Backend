namespace ScmssApiServer.DTOs
{
    public class PurchaseRequisitionUpdateDto
    {
        public ApprovalStatusOption? ApprovalStatus { get; set; }
        public bool? IsCanceled { get; set; }
        public ICollection<OrderItemInputDto>? Items { get; set; }
        public string? Problem { get; set; }
    }
}
