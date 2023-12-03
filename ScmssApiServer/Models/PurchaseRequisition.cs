using AutoMapper;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents a purchase requisition.
    /// </summary>
    public class PurchaseRequisition : StandardLifecycle, IApprovable
    {
        public ApprovalStatus ApprovalStatus { get; private set; }
        public User? ApproveFinance { get; private set; }
        public string? ApproveFinanceId { get; private set; }
        public User? ApproveProductionManager { get; private set; }
        public string? ApproveProductionManagerId { get; private set; }
        public int Id { get; set; }
        public override bool IsCreated => Id != 0;

        public ICollection<PurchaseRequisitionItem> Items { get; private set; }
                    = new List<PurchaseRequisitionItem>();

        public ProductionFacility ProductionFacility { get; set; } = null!;
        public int ProductionFacilityId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; private set; }
        public PurchaseRequisitionStatus Status { get; private set; }
        public decimal SubTotal { get; private set; }
        public ICollection<Supply> Supplies { get; private set; } = new List<Supply>();
        public decimal TotalAmount { get; private set; }
        public decimal VatAmount { get; private set; }
        public double VatRate { get; private set; }
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

            int duplicateCount = items.GroupBy(x => x.ItemId).Count(g => g.Count() > 1);
            if (duplicateCount > 0)
            {
                throw new InvalidDomainOperationException("Duplicate order item IDs found.");
            }

            Items.Clear();
            Items = items;
            CalculateTotals();
        }

        public virtual void Approve(string userId)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot approve purchase requisition which " +
                        "isn't currently waiting for approval."
                    );
            }
            ApprovalStatus = ApprovalStatus.Approved;
            ApproveProductionManagerId = userId;
            ApproveFinanceId = userId;
        }

        public override void Begin(string userId)
        {
            base.Begin(userId);
            ApprovalStatus = ApprovalStatus.PendingApproval;
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
            order.AddItems(orderItems);
            order.Begin(userId);

            PurchaseOrder = order;
            Status = PurchaseRequisitionStatus.Purchasing;

            return order;
        }

        public virtual void Reject(string userId, string problem)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot reject purchase requisition " +
                        "which isn't waiting for approval."
                    );
            }
            ApprovalStatus = ApprovalStatus.Rejected;
            EndWithProblem(userId, problem);
        }

        protected void CalculateTotals()
        {
            SubTotal = Items.Sum(i => i.TotalPrice);
            VatAmount = SubTotal * (decimal)VatRate;
            TotalAmount = SubTotal + VatAmount;
        }
    }

    public class PurchaseRequisitionMp : Profile
    {
        public PurchaseRequisitionMp()
        {
            CreateMap<PurchaseRequisition, PurchaseRequisitionDto>();
        }
    }
}
