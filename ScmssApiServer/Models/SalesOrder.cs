using AutoMapper;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class SalesOrder : Order<SalesOrderItem, SalesOrderEvent>
    {
        public ICollection<Product> Products { get; set; }
            = new List<Product>();

        private int? productionFacilityId = null;

        public int? ProductionFacilityId
        {
            get => productionFacilityId;
            set
            {
                if (productionFacilityId != null
                    && value != productionFacilityId
                    && Status != OrderStatus.Processing)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot change production facility after sales order has started delivery."
                        );
                }
                productionFacilityId = value;
            }
        }

        private ProductionFacility? productionFacility = null;

        public ProductionFacility? ProductionFacility
        {
            get => productionFacility;
            set
            {
                if (productionFacility != null
                    && value?.Id != productionFacility.Id
                    && Status != OrderStatus.Processing)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot change production facility after sales order has started delivery."
                        );
                }
                productionFacility = value;
            }
        }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public override void StartDelivery()
        {
            if (ProductionFacilityId == null)
            {
                throw new InvalidDomainOperationException(
                        "Cannot start delivery of order without starting production facility."
                    );
            }
            base.StartDelivery();
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
