using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    [PrimaryKey(nameof(WarehouseProductItemProductId),
                nameof(WarehouseProductItemProductionFacilityId),
                nameof(Time))]
    public class WarehouseProductItemEvent : WarehouseItemEvent
    {
        public ProductionOrder? ProductionOrder { get; set; }
        public int? ProductionOrderId { get; set; }
        public SalesOrder? SalesOrder { get; set; }
        public int? SalesOrderId { get; set; }
        public WarehouseProductItem WarehouseProductItem { get; set; } = null!;
        public int WarehouseProductItemProductId { get; set; }
        public int WarehouseProductItemProductionFacilityId { get; set; }
    }

    public class WarehouseProductItemEventMP : Profile
    {
        public WarehouseProductItemEventMP()
        {
            CreateMap<WarehouseProductItemEvent, WarehouseProductItemEventDto>();
        }
    }
}
