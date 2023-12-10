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
                if (value?.Id != productionFacility?.Id && IsExecutionStarted)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot change production facility after the order has started delivery."
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
                if (value != productionFacilityId && IsExecutionStarted)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot change production facility after the order has started delivery."
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
                        "Cannot start order delivery without source production facility."
                    );
            }

            base.StartExecution();

            foreach (SalesOrderItem item in Items)
            {
                WarehouseProductItem warehouseItem = item.Product.WarehouseProductItems.First(
                        i => i.ProductionFacilityId == ProductionFacilityId
                    );
                warehouseItem.Quantity -= item.Quantity;
            }
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
