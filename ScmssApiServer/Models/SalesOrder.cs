using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class SalesOrder : Order<SalesOrderItem>
    {
        public required string ToLocation { get; set; }

        public ICollection<Product> Products { get; set; }
            = new List<Product>();

        public int? ProductionFacilityId { get; set; }
        public ProductionFacility? ProductionFacility { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public ICollection<SalesOrderProgressUpdate> ProgressUpdates { get; }
            = new List<SalesOrderProgressUpdate>();
    }

    public class SalesOrderMappingProfile : Profile
    {
        public SalesOrderMappingProfile()
        {
            CreateMap<SalesOrderInputDto, SalesOrder>();
        }
    }
}
