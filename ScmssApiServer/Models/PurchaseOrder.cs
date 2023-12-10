using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents a purchase order.
    /// </summary>
    public class PurchaseOrder : TransOrder<PurchaseOrderItem, PurchaseOrderEvent>
    {
        public decimal AdditionalDiscount { get; set; } = 0;

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

        public override void Cancel(string userId, string problem)
        {
            base.Cancel(userId, problem);
            PurchaseRequisition.Delay(problem);
        }

        public override void Complete(string userId)
        {
            base.Complete(userId);

            foreach (PurchaseOrderItem item in Items)
            {
                WarehouseSupplyItem warehouseItem = item.Supply.WarehouseSupplyItems.First(
                        i => i.ProductionFacilityId == ProductionFacilityId
                    );
                warehouseItem.Quantity += item.Quantity;
            }

            PurchaseRequisition.Complete(userId);
        }

        /// <summary>
        /// Edit discount amount of order items.
        /// </summary>
        /// <param name="discounts">A dict of discount amount keyed by order item ID</param>
        public void EditItemDiscounts(IDictionary<int, decimal> discounts)
        {
            foreach (var item in Items)
            {
                if (discounts.ContainsKey(item.ItemId))
                {
                    item.Discount = discounts[item.ItemId];
                }
            }
            DiscountSubtotal = Items.Sum(i => i.Discount);
        }

        public override void Return(string userId, string problem)
        {
            base.Return(userId, problem);
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
