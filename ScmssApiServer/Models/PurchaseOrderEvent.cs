using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class PurchaseOrderEvent : TransOrderEvent
    {
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
    }

    public class PurchaseOrderProgressUpdateMappingProfile : Profile
    {
        public PurchaseOrderProgressUpdateMappingProfile()
        {
            CreateMap<OrderEventInputDto, PurchaseOrderEvent>();
        }
    }
}
