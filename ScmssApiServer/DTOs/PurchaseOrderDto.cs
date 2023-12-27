namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderDto : TransOrderDto<PurchaseOrderItemDto>
    {
        public decimal AdditionalDiscount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountSubtotal { get; set; }
        public string? InvoiceName { get; set; }
        public Uri? InvoiceUrl { get; set; }
        public bool IsDiscountUpdateAllowed { get; set; }
        public decimal NetSubtotal { get; set; }
        public required ProductionFacilityDto ProductionFacility { get; set; }
        public int ProductionFacilityId { get; set; }
        public int? PurchaseRequisitionId { get; set; }
        public string? ReceiptName { get; set; }
        public Uri? ReceiptUrl { get; set; }
        public CompanyDto Vendor { get; set; } = null!;
        public int VendorId { get; set; }
    }
}
