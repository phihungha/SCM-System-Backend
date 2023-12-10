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
        public Uri? InvoiceUrl { get; set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;
        public int ProductionFacilityId { get; set; }
        public PurchaseRequisition PurchaseRequisition { get; set; } = null!;
        public int? PurchaseRequisitionId { get; set; }
        public Uri? ReceiptUrl { get; set; }
        public ICollection<Supply> Supplies { get; protected set; } = new List<Supply>();
        public Vendor Vendor { get; set; } = null!;
        public int VendorId { get; set; }

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
            CalculateTotals();
        }

        public override void Cancel(string userId, string problem)
        {
            base.Cancel(userId, problem);
            PurchaseRequisition.Delay(problem);
        }

        public override void Return(string userId, string problem)
        {
            base.Return(userId, problem);
            PurchaseRequisition.Delay(problem);
        }

        protected override void CalculateTotals()
        {
            SubTotal = Items.Sum(i => i.NetPrice);
            decimal NetAmount = SubTotal - AdditionalDiscount;
            VatAmount = NetAmount * (decimal)VatRate;
            TotalAmount = NetAmount + VatAmount;
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
