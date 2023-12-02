using AutoMapper;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public class ProductionOrder : Order<ProductionOrderItem, ProductionOrderEvent>,
                                   IApprovable
    {
        public ApprovalStatus ApprovalStatus { get; set; }
        public User? ApproveProductionManager { get; set; }
        public string? ApproveProductionManagerId { get; set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;
        public int ProductionFacilityId { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal TotalValue { get; set; }

        public override void AddItem(ProductionOrderItem item)
        {
            base.AddItem(item);
            CalculateTotals();
        }

        public ProductionOrderEvent AddManualEvent(ProductionOrderEventTypeSelection typeSel,
                                                   string location,
                                                   string? message)
        {
            if (!IsExecuting)
            {
                throw new InvalidDomainOperationException(
                        "Cannot add manual event when order is not in production."
                    );
            }

            ProductionOrderEventType type;
            switch (typeSel)
            {
                case ProductionOrderEventTypeSelection.StageDone:
                    type = ProductionOrderEventType.StageDone;
                    Status = OrderStatus.Executing;
                    break;

                case ProductionOrderEventTypeSelection.Interrupted:
                    type = ProductionOrderEventType.Interrupted;
                    Status = OrderStatus.Interrupted;
                    break;

                default:
                    throw new ArgumentException("Invalid event type");
            }

            return AddEvent(type, location, message);
        }

        public void Approve(string userId)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot approve production order which isn't currently waiting for approval."
                    );
            }
            ApprovalStatus = ApprovalStatus.Approved;
            ApproveProductionManagerId = userId;
        }

        public override void Begin(string userId)
        {
            base.Begin(userId);
            ApprovalStatus = ApprovalStatus.PendingApproval;
            AddEvent(ProductionOrderEventType.PendingApproval);
        }

        public override void Cancel(string userId, string problem)
        {
            base.Cancel(userId, problem);
            ProductionOrderEvent lastEvent = Events.Last();
            AddEvent(ProductionOrderEventType.Canceled, lastEvent.Location);
        }

        public override void Complete(string userId)
        {
            base.Complete(userId);
            AddEvent(ProductionOrderEventType.Completed);
        }

        /// <summary>
        /// Finish production.
        /// </summary>
        public override void FinishExecution()
        {
            base.FinishExecution();
            AddEvent(ProductionOrderEventType.Produced);
        }

        public void Reject(string userId, string problem)
        {
            if (ApprovalStatus != ApprovalStatus.PendingApproval)
            {
                throw new InvalidDomainOperationException(
                        "Cannot reject production order that isn't waiting for approval"
                    );
            }
            ApprovalStatus = ApprovalStatus.Rejected;
            Problem = problem;
            End(userId);
        }

        public override void Return(string userId, string problem)
        {
            base.Return(userId, problem);
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
        }

        public ProductionOrderEvent UpdateEvent(int id, string? message = null, string? location = null)
        {
            if (IsEnded)
            {
                throw new InvalidDomainOperationException(
                        "Cannot update event because the order is finished."
                    );
            }

            ProductionOrderEvent? item = Events.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                throw new EntityNotFoundException();
            }

            if (location != null)
            {
                if (item.IsAutomatic)
                {
                    throw new InvalidDomainOperationException("Cannot edit location of automatic order event.");
                }
                item.Location = location;
            }
            item.Message = message;

            return item;
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

        private void CalculateTotals()
        {
            TotalValue = Items.Sum(i => i.TotalValue);
            TotalCost = Items.Sum(i => i.TotalCost);
            TotalProfit = TotalValue - TotalCost;
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
