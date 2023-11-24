using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class Customer : Company
    {
        public IList<SalesOrder> SalesOrders { get; set; }
            = new List<SalesOrder>();
    }

    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CompanyDto>();
        }
    }
}
