using AutoMapper;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class ProductionOrder : Order<ProductionOrderItem, ProductionOrderEvent>
    {
        public ApprovalStatus ApprovalStatus { get; protected set; }
        public User? ApproveProductionManager { get; protected set; }
        public string? ApproveProductionManagerId { get; protected set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;
        public int ProductionFacilityId { get; set; }
        public ICollection<Product> Products { get; protected set; } = new List<Product>();
        public ICollection<Supply> Supplies { get; protected set; } = new List<Supply>();

        public ICollection<ProductionOrderSupplyUsageItem> SupplyUsageItems { get; protected set; }
                    = new List<ProductionOrderSupplyUsageItem>();

        public decimal TotalCost { get; protected set; }

        public decimal TotalProfit
        {
            get => TotalValue - TotalCost;
            private set => _ = value;
        }

        public decimal TotalValue { get; protected set; }

        public ICollection<WarehouseProductItemEvent> WarehouseProductItemEvents { get; protected set; }
            = new List<WarehouseProductItemEvent>();

        public ICollection<WarehouseSupplyItemEvent> WarehouseSupplyItemEvents { get; protected set; }
            = new List<WarehouseSupplyItemEvent>();

        public override void AddItems(ICollection<ProductionOrderItem> items)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot add items after the order has been approved or rejected."
                );
            }

            if (!CheckStock(items))
            {
                throw new InvalidDomainOperationException(
                        "Not enough supply stock in the selected warehouse to produce the order items."
                    );
            }

            base.AddItems(items);

            var supplyUsageItems = new Dictionary<int, ProductionOrderSupplyUsageItem>();
            foreach (ProductionOrderItem item in items)
            {
                foreach (ProductSupplyCostItem costItem in item.Product.SupplyCostItems)
                {
                    Supply supply = costItem.Supply;
                    double supplyUsage = costItem.Quantity * item.Quantity;

                    if (!supplyUsageItems.ContainsKey(supply.Id))
                    {
                        supplyUsageItems[supply.Id] = new ProductionOrderSupplyUsageItem
                        {
                            SupplyId = supply.Id,
                            Quantity = supplyUsage,
                            Unit = supply.Unit,
                            UnitCost = supply.Price,
                        };
                    }
                    else
                    {
                        supplyUsageItems[supply.Id].Quantity += supplyUsage;
                    }
                }
            }
            SupplyUsageItems = supplyUsageItems.Values.ToList();

            TotalValue = Items.Sum(i => i.TotalValue);
            TotalCost = Items.Sum(i => i.TotalCost);
        }

        public ProductionOrderEvent AddManualEvent(ProductionOrderEventTypeOption typeSel,
                                                   string location,
                                                   string? message)
        {
            if (!IsExecuting)
            {
                throw new InvalidDomainOperationException(
                        "Cannot add a manual event when the order is not in production."
                    );
            }

            ProductionOrderEventType type;
            switch (typeSel)
            {
                case ProductionOrderEventTypeOption.StageDone:
                    type = ProductionOrderEventType.StageDone;
                    Status = OrderStatus.Executing;
                    break;

                case ProductionOrderEventTypeOption.Interrupted:
                    type = ProductionOrderEventType.Interrupted;
                    Status = OrderStatus.Interrupted;
                    break;

                default:
                    throw new ArgumentException("Invalid event type.");
            }

            return AddEvent(type, location, message);
        }

        public void Approve(User user)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot approve a production order which isn't waiting approval."
                    );
            }
            ApprovalStatus = ApprovalStatus.Approved;
            ApproveProductionManagerId = user.Id;
            ApproveProductionManager = user;
        }

        public override void Begin(User user)
        {
            base.Begin(user);
            ApprovalStatus = ApprovalStatus.PendingApproval;
            AddEvent(ProductionOrderEventType.PendingApproval);
        }

        public override void Cancel(User user, string problem)
        {
            base.Cancel(user, problem);
            ProductionOrderEvent lastEvent = Events.Last();
            AddEvent(ProductionOrderEventType.Canceled, lastEvent.Location);
        }

        public override void Complete(User user)
        {
            base.Complete(user);
            AddEvent(ProductionOrderEventType.Completed);

            foreach (ProductionOrderItem item in Items)
            {
                WarehouseProductItem warehouseItem = item.Product.WarehouseProductItems.First(
                        i => i.ProductionFacilityId == ProductionFacilityId
                    );
                warehouseItem.Quantity += item.Quantity;

                var warehouseEvent = new WarehouseProductItemEvent
                {
                    Time = DateTime.UtcNow,
                    Quantity = warehouseItem.Quantity,
                    Change = item.Quantity,
                    ProductionOrder = this,
                    ProductionOrderId = Id,
                    WarehouseProductItem = warehouseItem,
                    WarehouseProductItemId = warehouseItem.Id,
                };
                warehouseItem.Events.Add(warehouseEvent);
            }
        }

        /// <summary>
        /// Finish production.
        /// </summary>
        public override void FinishExecution()
        {
            base.FinishExecution();
            AddEvent(ProductionOrderEventType.Produced);
        }

        public void Reject(User user, string problem)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot reject a production order which isn't waiting approval."
                    );
            }
            ApprovalStatus = ApprovalStatus.Rejected;
            Cancel(user, problem);
        }

        public override void Return(User user, string problem)
        {
            base.Return(user, problem);
            AddEvent(ProductionOrderEventType.Unaccepted);
        }

        /// <summary>
        /// Start production.
        /// </summary>
        public override void StartExecution()
        {
            if (ApprovalStatus != ApprovalStatus.Approved)
            {
                throw new InvalidDomainOperationException("Cannot start an unapproved production order.");
            }
            base.StartExecution();
            AddEvent(ProductionOrderEventType.Producing);

            foreach (ProductionOrderSupplyUsageItem item in SupplyUsageItems)
            {
                WarehouseSupplyItem warehouseItem = item.Supply.WarehouseSupplyItems.First(
                    i => i.ProductionFacilityId == ProductionFacilityId
                );
                warehouseItem.Quantity -= item.Quantity;

                var warehouseEvent = new WarehouseSupplyItemEvent
                {
                    Time = DateTime.UtcNow,
                    Quantity = warehouseItem.Quantity,
                    Change = -item.Quantity,
                    ProductionOrder = this,
                    ProductionOrderId = Id,
                    WarehouseSupplyItem = warehouseItem,
                    WarehouseSupplyItemId = warehouseItem.Id,
                };
                warehouseItem.Events.Add(warehouseEvent);
            }
        }

        protected ProductionOrderEvent AddEvent(ProductionOrderEventType type, string? location = null, string? message = null)
        {
            var item = new ProductionOrderEvent
            {
                Type = type,
                Location = location,
                Time = DateTime.UtcNow,
                Message = message
            };
            Events.Add(item);
            return item;
        }

        private bool CheckStock(IEnumerable<ProductionOrderItem> items)
        {
            var totalSupplyUsage = new Dictionary<int, double>();
            var warehouseItems = new Dictionary<int, WarehouseSupplyItem>();

            foreach (ProductionOrderItem item in items)
            {
                foreach (ProductSupplyCostItem costItem in item.Product.SupplyCostItems)
                {
                    int supplyId = costItem.SupplyId;
                    double supplyUsage = costItem.Quantity * item.Quantity;

                    if (!totalSupplyUsage.ContainsKey(supplyId))
                    {
                        totalSupplyUsage[supplyId] = supplyUsage;
                        warehouseItems[supplyId] = costItem.Supply
                            .WarehouseSupplyItems
                            .First(i => i.ProductionFacilityId == ProductionFacilityId);
                    }
                    else
                    {
                        totalSupplyUsage[supplyId] += supplyUsage;
                    }
                }
            }

            return totalSupplyUsage.All(i => i.Value <= warehouseItems[i.Key].Quantity);
        }
    }

    public class ProductionOrderMp : Profile
    {
        public ProductionOrderMp()
        {
            CreateMap<ProductionOrder, ProductionOrderDto>();
        }
    }
}
