using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents a purchase order.
    /// </summary>
    public class PurchaseOrder : TransOrder<PurchaseOrderItem, PurchaseOrderEvent>
    {
        public decimal DiscountAmount { get; set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;
        public int ProductionFacilityId { get; set; }
        public PurchaseRequisition? PurchaseRequisition { get; set; }
        public int? PurchaseRequisitionId { get; set; }
        public ICollection<Supply> Supplies { get; set; } = new List<Supply>();
        public Vendor Vendor { get; set; } = null!;
        public int VendorId { get; set; }
    }

    public class PurchaseOrderMp : Profile
    {
        public PurchaseOrderMp()
        {
            CreateMap<PurchaseOrder, PurchaseOrderDto>();
        }
    }
}
