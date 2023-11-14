using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class SalesOrderItem : OrderItem
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; } = null!;
    }

    public class SalesOrderItemMappingProfile : Profile
    {
        public SalesOrderItemMappingProfile()
        {
            CreateMap<OrderItemInputDto, SalesOrderItem>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemId));
        }
    }
}
