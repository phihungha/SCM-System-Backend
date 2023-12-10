namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderUpdateDto : TransOrderUpdateDto<PurchaseOrderItemInputDto>
    {
        public decimal? AdditionalDiscount { get; set; }
        public string? FromLocation { get; set; }
        public Uri? InvoiceUrl { get; set; }
        public Uri? ReceiptUrl { get; set; }
    }
}
