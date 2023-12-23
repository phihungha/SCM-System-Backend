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

        public override bool IsExecutionStartAllowed =>
            base.IsExecutionStartAllowed && ProductionFacilityId != null;

        public ProductionFacility? ProductionFacility
        {
            get => productionFacility;
            set
            {
                if (value?.Id != productionFacility?.Id)
                {
                    if (!IsProcessing)
                    {
                        throw new InvalidDomainOperationException(
                                "Cannot change production facility if order is not in processing."
                            );
                    }

                    if (value != null && !CheckStock(Items, value.Id))
                    {
                        throw new InvalidDomainOperationException(
                                "Not enough stock in selected facility for the order items."
                            );
                    }
                }
                productionFacility = value;
            }
        }

        public int? ProductionFacilityId
        {
            get => productionFacilityId;
            set
            {
                if (value != productionFacilityId)
                {
                    if (!IsProcessing)
                    {
                        throw new InvalidDomainOperationException(
                                "Cannot change production facility if order is not in processing."
                            );
                    }

                    if (value != null && !CheckStock(Items, (int)value))
                    {
                        throw new InvalidDomainOperationException(
                                "Not enough stock in selected facility for the order items."
                            );
                    }
                }

                productionFacilityId = value;
            }
        }

        public ICollection<Product> Products { get; protected set; } = new List<Product>();

        public ICollection<WarehouseProductItemEvent> WarehouseProductItemEvents { get; protected set; }
                                    = new List<WarehouseProductItemEvent>();

        public override void AddItems(ICollection<SalesOrderItem> items)
        {
            if (ProductionFacility != null && !CheckStock(items, ProductionFacility))
            {
                throw new InvalidDomainOperationException(
                        "Not enough stock in selected facility for the order items."
                    );
            }
            base.AddItems(items);
        }

        public override void StartExecution()
        {
            if (ProductionFacilityId == null)
            {
                throw new InvalidDomainOperationException(
                        "Cannot start order delivery without source production facility."
                    );
            }

            if (!CheckStock(Items, (int)ProductionFacilityId))
            {
                throw new InvalidDomainOperationException(
                        "Not enough product stock in selected facility to issue."
                    );
            }

            base.StartExecution();

            foreach (SalesOrderItem item in Items)
            {
                WarehouseProductItem warehouseItem = item.Product.WarehouseProductItems.First(
                        i => i.ProductionFacilityId == ProductionFacilityId
                    );
                warehouseItem.IssueForSales(item.Quantity, this);
            }
        }

        private static bool CheckStock(IEnumerable<SalesOrderItem> items, int facilityId)
        {
            foreach (SalesOrderItem item in items)
            {
                WarehouseProductItem warehouseItem = item.Product.WarehouseProductItems.First(
                        i => i.ProductionFacilityId == facilityId
                    );
                if (item.Quantity > warehouseItem.Quantity)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CheckStock(IEnumerable<SalesOrderItem> items, ProductionFacility facility)
        {
            return CheckStock(items, facility.Id);
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
