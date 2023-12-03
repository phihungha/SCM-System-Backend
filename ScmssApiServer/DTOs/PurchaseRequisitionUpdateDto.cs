namespace ScmssApiServer.DTOs
{
    public class PurchaseRequisitionUpdateDto
    {
        public ApprovalStatusSelection? ApprovalStatus { get; set; }
        public bool? IsCanceled { get; set; }
        public ICollection<OrderItemInputDto>? Items { get; set; }
        public string? Problem { get; set; }
    }
}
