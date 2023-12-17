namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderUpdateDto : TransOrderUpdateDto<PurchaseOrderItemDiscountInputDto>
    {
        public decimal? AdditionalDiscount { get; set; }
        public string? FromLocation { get; set; }
        public bool? HasInvoice { get; set; }
        public bool? HasReceipt { get; set; }
    }
}
