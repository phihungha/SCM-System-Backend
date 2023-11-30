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
        private ProductionFacility? productionFacility = null;
        private int? productionFacilityId = null;
        public Customer Customer { get; set; } = null!;
        public int CustomerId { get; set; }

        public ProductionFacility? ProductionFacility
        {
            get => productionFacility;
            set
            {
                if (value?.Id != productionFacility?.Id && IsStarted)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot change production facility after order has started delivery."
                        );
                }
                productionFacility = value;
            }
        }

        public int? ProductionFacilityId
        {
            get => productionFacilityId;
            set
            {
                if (value != productionFacilityId && IsStarted)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot change production facility after order has started delivery."
                        );
                }
                productionFacilityId = value;
            }
        }

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
