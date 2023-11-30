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

        /// <summary>
        /// Events happening on the order.
        /// </summary>
        public ICollection<TEvent> Events { get; } = new List<TEvent>();

        public DateTime? ExecutionFinishTime { get; private set; }
        public DateTime? FinishTime { get; private set; }
        public User? FinishUser { get; private set; }
        public string? FinishUserId { get; private set; }
        public int Id { get; set; }
        public bool IsFinished { get => FinishTime != null; }
        public bool IsStarted => Status != OrderStatus.Processing;

        /// <summary>
        /// Order lines.
        /// </summary>
        public ICollection<TItem> Items { get; } = new List<TItem>();

        /// <summary>
        /// Reason the order was canceled or returned.
        /// </summary>
        public string? Problem { get; private set; }

        public OrderStatus Status { get; protected set; }
        public DateTime? UpdateTime { get; set; }

        public virtual void AddItem(TItem item)
        {
            if (IsStarted)
            {
                throw new InvalidDomainOperationException(
                        "Cannot add order item after order has started delivery."
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
        }

        public virtual void Cancel(string userId, string problem)
        {
            if (IsFinished || Status == OrderStatus.WaitingAcceptance)
            {
                throw new InvalidDomainOperationException(
                        "Cannot cancel an order which is waiting acceptance or is finished."
                    );
            }
            Status = OrderStatus.Canceled;
            Problem = problem;
            Finish(userId);
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
            Finish(userId);
        }

        public virtual void Finish(string userId)
        {
            if (FinishTime != null)
            {
                throw new InvalidDomainOperationException("Order is already finished");
            }

            FinishTime = DateTime.UtcNow;
            FinishUserId = userId;
        }

        public virtual void FinishExecution()
        {
            if (Status != OrderStatus.Executing)
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
            Finish(userId);
        }

        public virtual void StartExecution()
        {
            if (IsStarted)
            {
                throw new InvalidDomainOperationException(
                        "Cannot start delivery of order again."
                    );
            }
            Status = OrderStatus.Executing;
        }
    }
}
