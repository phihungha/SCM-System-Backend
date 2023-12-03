namespace ScmssApiServer.DTOs
{
    public class PurchaseRequisitionItemDto : TransOrderItemDto
    {
        public required SupplyDto Supply { get; set; }
    }
}
