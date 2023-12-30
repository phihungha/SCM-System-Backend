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

        public TimeSpan? ExecutionDuration
        {
            get => ExecutionFinishTime - CreateTime;
            set => _ = value;
        }

        public DateTime? ExecutionFinishTime { get; protected set; }
        public int Id { get; set; }
        public virtual bool IsAcceptAllowed => Status == OrderStatus.WaitingAcceptance;
        public virtual bool IsCancelAllowed => !IsExecutionFinished && !IsEnded;
        public override bool IsCreated => Id != 0;

        public bool IsExecuting => Status == OrderStatus.Executing
                                   || Status == OrderStatus.Interrupted;

        public virtual bool IsExecutionFinishAllowed => IsExecuting;
        public bool IsExecutionFinished => ExecutionFinishTime != null;
        public virtual bool IsExecutionInfoUpdateAllowed => IsProcessing;
        public virtual bool IsExecutionStartAllowed => IsProcessing;
        public bool IsProcessing => Status == OrderStatus.Processing;

        /// <summary>
        /// Order lines.
        /// </summary>
        public ICollection<TItem> Items { get; protected set; } = new List<TItem>();

        public OrderStatus Status { get; protected set; }

        public virtual void AddItems(ICollection<TItem> items)
        {
            if (!IsProcessing)
            {
                throw new InvalidDomainOperationException(
                        "Cannot add items when order is not in processing."
                );
            }

            int duplicateCount = items.GroupBy(x => x.ItemId).Count(g => g.Count() > 1);
            if (duplicateCount > 0)
            {
                throw new InvalidDomainOperationException("Duplicate order item IDs found.");
            }

            Items = items;
        }

        public override void Begin(User user)
        {
            base.Begin(user);
            Status = OrderStatus.Processing;
        }

        public virtual void Cancel(User user, string problem)
        {
            if (IsExecutionFinished || IsEnded)
            {
                throw new InvalidDomainOperationException(
                        "Cannot cancel an already executed or canceled order."
                    );
            }
            Status = OrderStatus.Canceled;
            EndWithProblem(user, problem);
        }

        public virtual void Complete(User user)
        {
            if (Status != OrderStatus.WaitingAcceptance)
            {
                throw new InvalidDomainOperationException(
                        "Cannot complete the order if it isn't waiting acceptance."
                    );
            }
            Status = OrderStatus.Completed;
            End(user);
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

        public virtual void Return(User user, string problem)
        {
            if (Status != OrderStatus.WaitingAcceptance)
            {
                throw new InvalidDomainOperationException(
                        "Cannot return the order if it isn't waiting acceptance."
                    );
            }
            Status = OrderStatus.Returned;
            EndWithProblem(user, problem);
        }

        public virtual void StartExecution()
        {
            if (!IsProcessing)
            {
                throw new InvalidDomainOperationException(
                        "Cannot start execution of an order that isn't in processing."
                    );
            }
            Status = OrderStatus.Executing;
        }

        public virtual TEvent UpdateEvent(int id, string? location = null, string? message = null)
        {
            TEvent? item = Events.SingleOrDefault(i => i.Id == id);
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
