namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderItemDto : TransOrderItemDto
    {
        public required SupplyDto Supply { get; set; }
        public decimal Discount { get; set; }
        public decimal NetPrice { get; set; }
    }
}
