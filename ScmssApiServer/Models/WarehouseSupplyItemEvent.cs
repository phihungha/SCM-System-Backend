using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    [PrimaryKey(nameof(WarehouseSupplyItemSupplyId),
                nameof(WarehouseSupplyItemProductionFacilityId),
                nameof(Time))]
    public class WarehouseSupplyItemEvent : WarehouseItemEvent
    {
        public ProductionOrder? ProductionOrder { get; set; }
        public int? ProductionOrderId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
        public int? PurchaseOrderId { get; set; }
        public WarehouseSupplyItem WarehouseSupplyItem { get; set; } = null!;
        public int WarehouseSupplyItemProductionFacilityId { get; set; }
        public int WarehouseSupplyItemSupplyId { get; set; }
    }

    public class WarehouseSupplyItemEventMP : Profile
    {
        public WarehouseSupplyItemEventMP()
        {
            CreateMap<WarehouseSupplyItemEvent, WarehouseSupplyItemEventDto>();
        }
    }
}
