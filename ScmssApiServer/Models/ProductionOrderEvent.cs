using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class ProductionOrderEvent : OrderEvent
    {
        public int ProductionOrderId { get; set; }
        public ProductionOrder ProductionOrder { get; set; } = null!;
    }

    public class ProductionOrderEventMP : Profile
    {
        public ProductionOrderEventMP()
        {
            CreateMap<OrderEventInputDto, ProductionOrderEvent>();
        }
    }
}
