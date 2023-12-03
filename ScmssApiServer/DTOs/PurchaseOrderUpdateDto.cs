namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderUpdateDto : TransOrderUpdateDto<PurchaseOrderItemUpdateDto>
    {
        public decimal? DiscountAmount { get; set; }
        public string? FromLocation { get; set; }
        public Uri? InvoiceUrl { get; set; }
        public Uri? ReceiptUrl { get; set; }
    }
}
