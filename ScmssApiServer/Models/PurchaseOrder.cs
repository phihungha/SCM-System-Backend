using AutoMapper;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents a purchase order.
    /// </summary>
    public class PurchaseOrder : TransOrder<PurchaseOrderItem, PurchaseOrderEvent>
    {
        private decimal additionalDiscount;

        public decimal AdditionalDiscount
        {
            get => additionalDiscount;
            set
            {
                if (value != additionalDiscount &&
                    PaymentStatus != TransOrderPaymentStatus.Pending)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot set additional discount after payment became due."
                        );
                }
                additionalDiscount = value;
            }
        }

        /// <summary>
        /// Total discount amount = DiscountSubtotal + AdditionalDiscount
        /// </summary>
        public decimal DiscountAmount
        {
            get => DiscountSubtotal + AdditionalDiscount;
            private set => _ = value;
        }

        /// <summary>
        /// Sum of PurchaseOrderItem.Discount
        /// </summary>
        public decimal DiscountSubtotal { get; set; }

        public Uri? InvoiceUrl { get; set; }

        /// <summary>
        /// Subtotal after discount.
        /// </summary>
        public decimal NetSubtotal
        {
            get => SubTotal - DiscountAmount;
            private set => _ = value;
        }

        public ProductionFacility ProductionFacility { get; set; } = null!;

        public int ProductionFacilityId { get; set; }

        public PurchaseRequisition PurchaseRequisition { get; set; } = null!;

        public int? PurchaseRequisitionId { get; set; }

        public Uri? ReceiptUrl { get; set; }

        public ICollection<Supply> Supplies { get; protected set; } = new List<Supply>();

        /// <summary>
        /// Total amount to pay = NetSubtotal + VatAmount
        /// </summary>
        public override decimal TotalAmount
        {
            get => NetSubtotal + VatAmount;
        }

        /// <summary>
        /// VAT-taxed amount = NetSubtotal * VatRate
        /// </summary>
        public override decimal VatAmount
        {
            get => NetSubtotal * (decimal)VatRate;
        }

        public Vendor Vendor { get; set; } = null!;

        public int VendorId { get; set; }

        public ICollection<WarehouseSupplyItemEvent> WarehouseSupplyItemEvents { get; protected set; }
            = new List<WarehouseSupplyItemEvent>();

        public override void Cancel(User user, string problem)
        {
            base.Cancel(user, problem);
            PurchaseRequisition.Delay(problem);
        }

        public override void Complete(User user)
        {
            base.Complete(user);

            foreach (PurchaseOrderItem item in Items)
            {
                WarehouseSupplyItem warehouseItem = item.Supply.WarehouseSupplyItems.First(
                        i => i.ProductionFacilityId == ProductionFacilityId
                    );
                warehouseItem.Quantity += item.Quantity;

                var warehouseEvent = new WarehouseSupplyItemEvent
                {
                    Time = DateTime.UtcNow,
                    Quantity = warehouseItem.Quantity,
                    Change = item.Quantity,
                    PurchaseOrder = this,
                    PurchaseOrderId = Id,
                    WarehouseSupplyItem = warehouseItem,
                    WarehouseSupplyItemSupplyId = warehouseItem.SupplyId,
                    WarehouseSupplyItemProductionFacilityId = warehouseItem.ProductionFacilityId,
                };
                warehouseItem.Events.Add(warehouseEvent);
            }

            PurchaseRequisition.Complete(user);
        }

        /// <summary>
        /// Edit discount amount of order items.
        /// </summary>
        /// <param name="discounts">A dict of discount amount keyed by order item ID</param>
        public void EditItemDiscounts(IDictionary<int, decimal> discounts)
        {
            if (PaymentStatus != TransOrderPaymentStatus.Pending)
            {
                throw new InvalidDomainOperationException(
                        "Cannot edit item discounts after payment became due."
                    );
            }

            foreach (var item in Items)
            {
                if (discounts.ContainsKey(item.ItemId))
                {
                    item.Discount = discounts[item.ItemId];
                }
            }
            DiscountSubtotal = Items.Sum(i => i.Discount);
        }

        public override void Return(User user, string problem)
        {
            base.Return(user, problem);
            PurchaseRequisition.Delay(problem);
        }
    }

    public class PurchaseOrderMp : Profile
    {
        public PurchaseOrderMp()
        {
            CreateMap<PurchaseOrder, PurchaseOrderDto>();
        }
    }
}
