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
        public ICollection<TEvent> Events { get; } = new List<TEvent>();

        public DateTime? ExecutionFinishTime { get; private set; }
        public int Id { get; set; }

        public override bool IsCreated => Id != 0;

        public bool IsExecuting => Status == OrderStatus.Executing
                                                   || Status == OrderStatus.Interrupted;

        public bool IsExecutionFinished => ExecutionFinishTime != null;
        public bool IsExecutionStarted => Status != OrderStatus.Processing;

        /// <summary>
        /// Order lines.
        /// </summary>
        public ICollection<TItem> Items { get; } = new List<TItem>();

        public OrderStatus Status { get; protected set; }

        public virtual void AddItem(TItem item)
        {
            if (IsExecutionStarted)
            {
                throw new InvalidDomainOperationException(
                        "Cannot add order item after order has started execution."
                    );
            }

            bool idAlreadyExists = Items.FirstOrDefault(
                    i => i.ItemId == item.ItemId
                ) != null;
            if (idAlreadyExists)
            {
                throw new InvalidDomainOperationException("An order item with this ID already exists");
            }

            Items.Add(item);
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
                        "Cannot complete order if it isn't waiting acceptance."
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
                        "Cannot finish execution of order if it is not being executed."
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
                        "Cannot complete order if it isn't waiting acceptance."
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
                        "Cannot start execution of order again."
                    );
            }
            Status = OrderStatus.Executing;
        }
    }
}
