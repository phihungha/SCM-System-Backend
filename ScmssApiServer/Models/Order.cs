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

        public bool Finished { get => FinishTime != null; }
        public DateTime? FinishTime { get; private set; }
        public User? FinishUser { get; private set; }
        public string? FinishUserId { get; private set; }
        public int Id { get; set; }

        /// <summary>
        /// Order lines.
        /// </summary>
        public ICollection<TItem> Items { get; } = new List<TItem>();

        public DateTime? UpdateTime { get; set; }

        public virtual void Finish(string userId)
        {
            if (FinishTime != null)
            {
                throw new InvalidDomainOperationException("Order is finished");
            }

            FinishTime = DateTime.UtcNow;
            FinishUserId = userId;
        }

        public virtual void Start(string userId)
        {
            if (Id != 0)
            {
                throw new InvalidDomainOperationException("Cannot start an already created order");
            }
            CreateUserId = userId;
        }
    }
}
