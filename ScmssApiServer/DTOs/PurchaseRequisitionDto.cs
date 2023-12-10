using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class PurchaseRequisitionDto : StandardLifecycleDto, IApprovableDto
    {
        public ApprovalStatus ApprovalStatus { get; set; }
        public UserDto? ApproveFinance { get; set; }
        public string? ApproveFinanceId { get; set; }
        public UserDto? ApproveProductionManager { get; set; }
        public string? ApproveProductionManagerId { get; set; }
        public int Id { get; set; }
        public required ICollection<PurchaseRequisitionItemDto> Items { get; set; }
        public required ProductionFacilityDto ProductionFacility { get; set; }
        public int ProductionFacilityId { get; set; }
        public ICollection<PurchaseOrderDto> PurchaseOrders { get; set; } = null!;
        public PurchaseRequisitionStatus Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal VatAmount { get; set; }
        public double VatRate { get; set; }
        public required CompanyDto Vendor { get; set; }
        public int VendorId { get; set; }
    }
}
