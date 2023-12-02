using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class SalesOrderEvent : TransOrderEvent
    {
        public int SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; } = null!;
    }

    public class SalesOrderEventMP : Profile
    {
        public SalesOrderEventMP()
        {
            CreateMap<SalesOrderEvent, TransOrderEventDto>();
            CreateMap<TransOrderEventCreateDto, SalesOrderEvent>();
        }
    }
}
