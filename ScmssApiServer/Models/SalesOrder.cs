using AutoMapper;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents a sales order.
    /// </summary>
    public class SalesOrder : TransOrder<SalesOrderItem, SalesOrderEvent>
    {
        public Customer Customer { get; set; } = null!;

        public int CustomerId { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public override void StartExecution()
        {
            if (ProductionFacilityId == null)
            {
                throw new InvalidDomainOperationException(
                        "Cannot start delivery of order without starting production facility."
                    );
            }
            base.StartExecution();
        }
    }

    public class SalesOrderMp : Profile
    {
        public SalesOrderMp()
        {
            CreateMap<SalesOrder, SalesOrderDto>();
        }
    }
}
