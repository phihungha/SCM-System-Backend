using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class SalesOrderEvent : OrderEvent
    {
        public int SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; } = null!;
    }

    public class SalesOrderEventMappingProfile : Profile
    {
        public SalesOrderEventMappingProfile()
        {
            CreateMap<SalesOrderEvent, OrderEventDto>();
            CreateMap<OrderEventCreateDto, SalesOrderEvent>();
        }
    }
}
