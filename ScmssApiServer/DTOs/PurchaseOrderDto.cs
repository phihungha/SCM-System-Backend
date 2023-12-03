namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderDto : TransOrderDto<PurchaseOrderItemDto>
    {
        public decimal DiscountAmount { get; set; }
        public required ProductionFacilityDto ProductionFacility { get; set; }
        public int ProductionFacilityId { get; set; }
        public int? PurchaseRequisitionId { get; set; }
        public CompanyDto Vendor { get; set; } = null!;
        public int VendorId { get; set; }
    }
}
