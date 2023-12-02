using ScmssApiServer.DomainExceptions;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents a purchase requisition.
    /// </summary>
    public class PurchaseRequisition : ApprovableLifecycle
    {
        public User? ApproveFinance { get; set; }
        public string? ApproveFinanceId { get; set; }
        public User? ApproveProductionManager { get; set; }
        public string? ApproveProductionManagerId { get; set; }
        public int Id { get; set; }
        public override bool IsCreated => Id != 0;

        public ICollection<PurchaseRequisitionItem> Items { get; set; }
                    = new List<PurchaseRequisitionItem>();

        public ProductionFacility ProductionFacility { get; set; } = null!;
        public int ProductionFacilityId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
        public PurchaseRequisitionStatus Status { get; set; }
        public decimal SubTotal { get; set; }
        public ICollection<Supply> Supplies { get; set; } = new List<Supply>();
        public decimal TotalAmount { get; set; }
        public decimal VatAmount { get; set; }
        public double VatRate { get; set; }
        public Vendor Vendor { get; set; } = null!;
        public int VendorId { get; set; }

        public void AddItems(ICollection<PurchaseRequisitionItem> items)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot add order item after requisition has been approved."
                    );
            }

            foreach (PurchaseRequisitionItem item in items)
            {
                bool idAlreadyExists = Items.FirstOrDefault(
                    i => i.ItemId == item.ItemId
                ) != null;
                if (idAlreadyExists)
                {
                    throw new InvalidDomainOperationException("An order item with this ID already exists.");
                }
            }

            Items = items;
        }

        public override void Approve(string userId)
        {
            base.Approve(userId);
            ApproveProductionManagerId = userId;
            ApproveFinanceId = userId;
        }

        public void Cancel(string userId, string problem)
        {
            Status = PurchaseRequisitionStatus.Canceled;
            EndWithProblem(userId, problem);
        }

        public PurchaseOrder GeneratePurchaseOrder(string userId)
        {
            var order = new PurchaseOrder
            {
                PurchaseRequisitionId = Id,
                VendorId = VendorId,
                ProductionFacilityId = ProductionFacilityId,
                CreateUserId = userId,
                ToLocation = ProductionFacility.Location,
            };

            var orderItems = Items.Select(i => new PurchaseOrderItem
            {
                ItemId = i.ItemId,
                Quantity = i.Quantity,
                Unit = i.Unit,
                UnitPrice = i.UnitPrice,
            }).ToList();
            order.AddItem(orderItems[0]);
            order.Begin(userId);

            Status = PurchaseRequisitionStatus.Purchasing;

            return order;
        }
    }
}
