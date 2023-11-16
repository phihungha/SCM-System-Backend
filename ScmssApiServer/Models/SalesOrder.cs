using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class SalesOrder : Order<SalesOrderItem, SalesOrderEvent>
    {
        public ICollection<Product> Products { get; set; }
            = new List<Product>();

        public int? ProductionFacilityId { get; set; }
        public ProductionFacility? ProductionFacility { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
    }

    public class SalesOrderMp : Profile
    {
        public SalesOrderMp()
        {
            CreateMap<SalesOrder, SalesOrderDto>();
            CreateMap<SalesOrderInputDto, SalesOrder>().ForMember(i => i.Items, o => o.Ignore());
            CreateMap<SalesOrderCreateDto, SalesOrder>().ForMember(i => i.Items, o => o.Ignore());
        }
    }
}
