namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderUpdateDto : TransOrderUpdateDto<PurchaseOrderItemDiscountInputDto>
    {
        public decimal? AdditionalDiscount { get; set; }
        public string? FromLocation { get; set; }
        public string? InvoiceName { get; set; }
        public string? ReceiptName { get; set; }
    }
}
