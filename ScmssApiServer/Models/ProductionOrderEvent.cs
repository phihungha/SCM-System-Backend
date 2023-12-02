using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class ProductionOrderEvent : OrderEvent
    {
        public override bool IsAutomatic => Type != ProductionOrderEventType.StageDone
                                            && Type != ProductionOrderEventType.Interrupted;

        public ProductionOrder ProductionOrder { get; set; } = null!;
        public int ProductionOrderId { get; set; }
        public ProductionOrderEventType Type { get; set; }
    }

    public class ProductionOrderEventMP : Profile
    {
        public ProductionOrderEventMP()
        {
            CreateMap<ProductionOrderEvent, ProductionOrderEventDto>();
            CreateMap<OrderEventCreateDto, ProductionOrderEvent>();
        }
    }
}
