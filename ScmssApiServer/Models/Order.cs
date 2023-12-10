using ScmssApiServer.DomainExceptions;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents a generic order.
    /// </summary>
    /// <typeparam name="TItem">Order line item type</typeparam>
    /// <typeparam name="TEvent">Order event type</typeparam>
    public abstract class Order<TItem, TEvent> : StandardLifecycle
        where TItem : OrderItem
        where TEvent : OrderEvent, new()
    {
        /// <summary>
        /// Events happening on the order.
        /// </summary>
        public ICollection<TEvent> Events { get; protected set; } = new List<TEvent>();

        public DateTime? ExecutionFinishTime { get; protected set; }
        public int Id { get; set; }

        public override bool IsCreated => Id != 0;

        public bool IsExecuting => Status == OrderStatus.Executing
                                   || Status == OrderStatus.Interrupted;

        public bool IsExecutionFinished => ExecutionFinishTime != null;
        public bool IsExecutionStarted => Status != OrderStatus.Processing;

        /// <summary>
        /// Order lines.
        /// </summary>
        public ICollection<TItem> Items { get; protected set; } = new List<TItem>();

        public OrderStatus Status { get; protected set; }

        public virtual void AddItems(ICollection<TItem> items)
        {
            if (IsExecutionStarted)
            {
                throw new InvalidDomainOperationException(
                        "Cannot add items after the order has started execution."
                );
            }

            int duplicateCount = items.GroupBy(x => x.ItemId).Count(g => g.Count() > 1);
            if (duplicateCount > 0)
            {
                throw new InvalidDomainOperationException("Duplicate order item IDs found.");
            }

            Items = items;
        }

        public override void Begin(string userId)
        {
            base.Begin(userId);
            Status = OrderStatus.Processing;
        }

        public virtual void Cancel(string userId, string problem)
        {
            if (IsExecutionFinished)
            {
                throw new InvalidDomainOperationException(
                        "Cannot cancel an already executed order."
                    );
            }
            Status = OrderStatus.Canceled;
            EndWithProblem(userId, problem);
        }

        public virtual void Complete(string userId)
        {
            if (Status != OrderStatus.WaitingAcceptance)
            {
                throw new InvalidDomainOperationException(
                        "Cannot complete the order if it isn't waiting acceptance."
                    );
            }
            Status = OrderStatus.Completed;
            End(userId);
        }

        public virtual void FinishExecution()
        {
            if (!IsExecuting)
            {
                throw new InvalidDomainOperationException(
                        "Cannot finish execution of the order if isn't being executed."
                    );
            }
            Status = OrderStatus.WaitingAcceptance;
            ExecutionFinishTime = DateTime.UtcNow;
        }

        public virtual void Return(string userId, string problem)
        {
            if (Status != OrderStatus.WaitingAcceptance)
            {
                throw new InvalidOperationException(
                        "Cannot return the order if it isn't waiting acceptance."
                    );
            }
            Status = OrderStatus.Returned;
            EndWithProblem(userId, problem);
        }

        public virtual void StartExecution()
        {
            if (IsExecutionStarted)
            {
                throw new InvalidDomainOperationException(
                        "Cannot start execution of the order again."
                    );
            }
            Status = OrderStatus.Executing;
        }

        public virtual TEvent UpdateEvent(int id, string? message = null, string? location = null)
        {
            if (IsEnded)
            {
                throw new InvalidDomainOperationException(
                        "Cannot update an event of an ended order."
                    );
            }

            TEvent? item = Events.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                throw new EntityNotFoundException();
            }

            if (location != null)
            {
                if (item.IsAutomatic)
                {
                    throw new InvalidDomainOperationException("Cannot edit location of an automatic order event.");
                }
                item.Location = location;
            }
            item.Message = message;

            return item;
        }
    }
}
