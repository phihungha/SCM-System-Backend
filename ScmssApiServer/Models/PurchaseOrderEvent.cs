using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class PurchaseOrderEvent : TransOrderEvent
    {
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        public int PurchaseOrderId { get; set; }
    }

    public class PurchaseOrderEventMP : Profile
    {
        public PurchaseOrderEventMP()
        {
            CreateMap<PurchaseOrderEvent, TransOrderEventDto>();
            CreateMap<TransOrderEventCreateDto, PurchaseOrderEvent>();
        }
    }
}
