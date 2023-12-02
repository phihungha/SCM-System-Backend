using ScmssApiServer.DomainExceptions;

namespace ScmssApiServer.Models
{
    public abstract class Order<TItem, TEvent> : ILifecycle
        where TItem : OrderItem
        where TEvent : OrderEvent, new()
    {
        public DateTime CreateTime { get; set; }
        public User CreateUser { get; set; } = null!;
        public required string CreateUserId { get; set; }
        public DateTime? EndTime { get; private set; }
        public User? EndUser { get; private set; }
        public string? EndUserId { get; private set; }

        /// <summary>
        /// Events happening on the order.
        /// </summary>
        public ICollection<TEvent> Events { get; } = new List<TEvent>();

        public DateTime? ExecutionFinishTime { get; private set; }
        public int Id { get; set; }
        public bool IsEnded { get => EndTime != null; }

        public bool IsExecuting => Status == OrderStatus.Executing
                                                   || Status == OrderStatus.Interrupted;

        public bool IsExecutionFinished => ExecutionFinishTime != null;
        public bool IsExecutionStarted => Status != OrderStatus.Processing;

        /// <summary>
        /// Order lines.
        /// </summary>
        public ICollection<TItem> Items { get; } = new List<TItem>();

        /// <summary>
        /// Reason the order was canceled or returned.
        /// </summary>
        public string? Problem { get; protected set; }

        public OrderStatus Status { get; protected set; }
        public DateTime? UpdateTime { get; set; }

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

        public virtual void Begin(string userId)
        {
            if (Id != 0)
            {
                throw new InvalidDomainOperationException("Cannot begin an already created order");
            }
            CreateUserId = userId;
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
            Problem = problem;
            End(userId);
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

        public virtual void End(string userId)
        {
            if (IsEnded)
            {
                throw new InvalidDomainOperationException("Order is already ended");
            }

            EndTime = DateTime.UtcNow;
            EndUserId = userId;
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
            Problem = problem;
            End(userId);
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
