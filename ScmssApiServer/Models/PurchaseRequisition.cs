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
        public ApprovalStatus ApprovalStatus { get; protected set; }
        public User? ApproveFinance { get; protected set; }
        public string? ApproveFinanceId { get; protected set; }
        public User? ApproveProductionManager { get; protected set; }
        public string? ApproveProductionManagerId { get; protected set; }
        public int Id { get; set; }
        public override bool IsCreated => Id != 0;

        public ICollection<PurchaseRequisitionItem> Items { get; protected set; }
                    = new List<PurchaseRequisitionItem>();

        public ProductionFacility ProductionFacility { get; set; } = null!;
        public int ProductionFacilityId { get; set; }

        public ICollection<PurchaseOrder> PurchaseOrders { get; protected set; }
            = new List<PurchaseOrder>();

        public PurchaseRequisitionStatus Status { get; protected set; }
        public decimal SubTotal { get; private set; }
        public ICollection<Supply> Supplies { get; protected set; } = new List<Supply>();
        public decimal TotalAmount { get; protected set; }
        public decimal VatAmount { get; protected set; }
        public double VatRate { get; protected set; }
        public Vendor Vendor { get; set; } = null!;
        public int VendorId { get; set; }

        public void AddItems(ICollection<PurchaseRequisitionItem> items)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot add items after the requisition has been approved or rejected."
                );
            }

            int duplicateCount = items.GroupBy(x => x.ItemId).Count(g => g.Count() > 1);
            if (duplicateCount > 0)
            {
                throw new InvalidDomainOperationException("Duplicate requisition item IDs found.");
            }

            Items = items;
            CalculateTotals();
        }

        public virtual void Approve(string userId)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot approve a purchase requisition which isn't waiting approval."
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
            if (IsEnded)
            {
                throw new InvalidDomainOperationException(
                        "Cannot cancel an ended purchase requisition."
                    );
            }
            Status = PurchaseRequisitionStatus.Canceled;
            EndWithProblem(userId, problem);
        }

        public void Delay(string problem)
        {
            if (Status != PurchaseRequisitionStatus.Purchasing)
            {
                throw new InvalidDomainOperationException(
                        "Cannot delay a purchase requisition which hasn't started execution."
                    );
            }
            Status = PurchaseRequisitionStatus.Delayed;
            Problem = problem;
        }

        public PurchaseOrder GeneratePurchaseOrder(string userId)
        {
            var order = new PurchaseOrder
            {
                PurchaseRequisitionId = Id,
                VendorId = VendorId,
                ProductionFacilityId = ProductionFacilityId,
                CreateUserId = userId,
                FromLocation = Vendor.DefaultLocation,
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

            PurchaseOrders.Add(order);
            Status = PurchaseRequisitionStatus.Purchasing;

            return order;
        }

        public virtual void Reject(string userId, string problem)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot reject a purchase requisition which isn't waiting approval."
                    );
            }
            ApprovalStatus = ApprovalStatus.Rejected;
            Cancel(userId, problem);
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
