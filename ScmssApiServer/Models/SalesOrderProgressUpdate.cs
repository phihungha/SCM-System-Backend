using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class SalesOrderProgressUpdate : OrderProgressUpdate
    {
        public int SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; } = null!;
    }

    public class SalesOrderProgressUpdateMappingProfile : Profile
    {
        public SalesOrderProgressUpdateMappingProfile()
        {
            CreateMap<OrderProgressUpdateInputDto, SalesOrderProgressUpdate>();
        }
    }
}
