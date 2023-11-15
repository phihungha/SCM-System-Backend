using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class ProductionOrderProgressUpdate : OrderEvent
    {
        public int ProductionOrderId { get; set; }
        public ProductionOrder ProductionOrder { get; set; } = null!;
    }

    public class ProductionOrderProgressUpdateMappingProfile : Profile
    {
        public ProductionOrderProgressUpdateMappingProfile()
        {
            CreateMap<OrderEventInputDto, ProductionOrderProgressUpdate>();
        }
    }
}
