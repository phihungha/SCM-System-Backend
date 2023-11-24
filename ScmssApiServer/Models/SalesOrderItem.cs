using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class SalesOrderItem : OrderItem
    {
        public Product Product { get; set; } = null!;
        public SalesOrder SalesOrder { get; set; } = null!;
    }

    public class SalesOrderItemMp : Profile
    {
        public SalesOrderItemMp()
        {
            CreateMap<SalesOrderItem, SalesOrderItemDto>();
        }
    }
}
