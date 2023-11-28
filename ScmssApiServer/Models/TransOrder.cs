using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents a transacton order such as a sales or purchase order.
    /// </summary>
    /// <typeparam name="TItem">Order line item type</typeparam>
    /// <typeparam name="TEvent">Order event type</typeparam>
    public abstract class TransOrder<TItem, TEvent> : ILifecycle
        where TItem : TransOrderItem
        where TEvent : OrderEvent, new()
    {
        public DateTime CreateTime { get; set; }
        public User CreateUser { get; set; } = null!;
        public required string CreateUserId { get; set; }
        public DateTime? DeliverTime { get; protected set; }
        public ICollection<TEvent> Events { get; } = new List<TEvent>();
        public bool Finished { get => FinishTime != null; }
        public DateTime? FinishTime { get; private set; }
        public User? FinishUser { get; private set; }
        public string? FinishUserId { get; private set; }

        /// <summary>
        /// Delivery start location.
        /// </summary>
        public string? FromLocation { get; set; }

        public int Id { get; set; }
        public string? InvoiceUrl { get; set; }

        /// <summary>
        /// Order item.
        /// </summary>
        public ICollection<TItem> Items { get; } = new List<TItem>();

        public TransOrderPaymentStatus PaymentStatus { get; private set; }

        /// <summary>
        /// Reason the order was canceled or returned.
        /// </summary>
        public string? Problem { get; private set; }

        public string? ReceiptUrl { get; set; }

        /// <summary>
        /// Remaining amount to pay.
        /// </summary>
        public decimal RemainingAmount { get; private set; }

        public TransOrderStatus Status { get; private set; }

        /// <summary>
        /// Sum of TransOrderItem.TotalPrice.
        /// </summary>
        public decimal SubTotal { get; private set; }

        /// <summary>
        /// Delivery destination location.
        /// </summary>
        public required string ToLocation { get; set; }

        /// <summary>
        /// Total amount to pay = SubTotal + VatAmount
        /// </summary>
        public decimal TotalAmount { get; private set; }

        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// VAT-taxed amount = SubTotal * VatRate
        /// </summary>
        public decimal VatAmount { get; private set; }

        /// <summary>
        /// VAT tax rate from 0 (0%) to 1 (100%).
        /// </summary>
        public double VatRate { get; private set; }

        public void AddItem(TItem item)
        {
            if (Status != TransOrderStatus.Processing)
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
            CalculateTotals();
        }

        public TEvent AddManualEvent(OrderEventTypeSelection typeSel, string location, string? message)
        {
            if (Status != TransOrderStatus.Delivering)
            {
                throw new InvalidDomainOperationException(
                        "Cannot add manual event when order is not being delivered."
                    );
            }

            OrderEventType type;
            switch (typeSel)
            {
                case OrderEventTypeSelection.Left:
                    type = OrderEventType.Left;
                    break;

                case OrderEventTypeSelection.Arrived:
                    type = OrderEventType.Arrived;
                    break;

                case OrderEventTypeSelection.Interrupted:
                    type = OrderEventType.Interrupted;
                    break;

                default:
                    throw new ArgumentException("Invalid event type");
            }

            return AddEvent(type, location, message);
        }

        public void Cancel(string userId, string problem)
        {
            if (Status == TransOrderStatus.Delivered || Status == TransOrderStatus.Completed)
            {
                throw new InvalidOperationException(
                        "Cannot cancel order after it has been delivered."
                    );
            }
            Status = TransOrderStatus.Canceled;
            PaymentStatus = TransOrderPaymentStatus.Canceled;
            Problem = problem;
            Finish(userId);

            TEvent lastEvent = Events.Last();
            AddEvent(OrderEventType.Canceled, lastEvent.Location);
        }

        public void Complete(string userId)
        {
            if (Status != TransOrderStatus.Delivered)
            {
                throw new InvalidDomainOperationException(
                        "Cannot complete order if it hasn't finished delivery."
                    );
            }
            Status = TransOrderStatus.Completed;
            AddEvent(OrderEventType.Completed, ToLocation);
            CreateDuePayment();
            Finish(userId);
        }

        public void CompletePayment(decimal amount)
        {
            if (PaymentStatus != TransOrderPaymentStatus.Due)
            {
                throw new InvalidDomainOperationException(
                        "Cannot complete payment of order if there is no due payment."
                    );
            }
            RemainingAmount = TotalAmount - amount;
            if (RemainingAmount == 0)
            {
                PaymentStatus = TransOrderPaymentStatus.Completed;
                AddEvent(OrderEventType.PaymentCompleted);
            }
        }

        public void Finish(string userId)
        {
            if (FinishTime != null)
            {
                throw new InvalidDomainOperationException("Order is finished");
            }

            FinishTime = DateTime.UtcNow;
            FinishUserId = userId;
        }

        public void FinishDelivery()
        {
            if (Status != TransOrderStatus.Delivering)
            {
                throw new InvalidDomainOperationException(
                        "Cannot finish delivery of order if it is not being delivered."
                    );
            }
            Status = TransOrderStatus.Delivered;
            DeliverTime = DateTime.UtcNow;
            AddEvent(OrderEventType.Delivered, ToLocation);
        }

        public void Return(string userId, string problem)
        {
            if (Status != TransOrderStatus.Delivered)
            {
                throw new InvalidOperationException(
                        "Cannot return order if it has been completed or hasn't finished delivery."
                    );
            }
            Status = TransOrderStatus.Returned;
            PaymentStatus = TransOrderPaymentStatus.Canceled;
            Problem = problem;
            Finish(userId);
            AddEvent(OrderEventType.Returned, ToLocation);
        }

        public void Start(string userId)
        {
            if (Id != 0)
            {
                throw new InvalidDomainOperationException("Cannot start an already created order");
            }

            Status = TransOrderStatus.Processing;
            PaymentStatus = TransOrderPaymentStatus.Pending;
            CreateUserId = userId;
            AddEvent(OrderEventType.Processing);
        }

        public virtual void StartDelivery()
        {
            if (Status != TransOrderStatus.Processing)
            {
                throw new InvalidDomainOperationException(
                        "Cannot start delivery of order again."
                    );
            }
            if (FromLocation == null)
            {
                throw new InvalidDomainOperationException(
                        "Cannot start delivery of order without destination location."
                    );
            }
            Status = TransOrderStatus.Delivering;
            AddEvent(OrderEventType.DeliveryStarted, FromLocation);
        }

        public TEvent UpdateEvent(int id, string? message = null, string? location = null)
        {
            if (FinishTime != null)
            {
                throw new InvalidDomainOperationException(
                        "Cannot update event because the order is finished."
                    );
            }

            TEvent? item = Events.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                throw new EntityNotFoundException();
            }

            if (location != null)
            {
                if (item.Type != OrderEventType.Left
                    && item.Type != OrderEventType.Arrived
                    && item.Type != OrderEventType.Interrupted)
                {
                    throw new InvalidDomainOperationException("Cannot edit location of automatic order event.");
                }
                item.Location = location;
            }
            item.Message = message;

            return item;
        }

        protected TEvent AddEvent(OrderEventType type, string? location = null, string? message = null)
        {
            var item = new TEvent
            {
                Type = type,
                Location = location,
                Time = DateTime.UtcNow,
                Message = message
            };
            Events.Add(item);
            return item;
        }

        protected void CalculateTotals()
        {
            SubTotal = Items.Sum(i => i.TotalPrice);
            VatAmount = SubTotal * (decimal)VatRate;
            TotalAmount = SubTotal + VatAmount;
        }

        private void CreateDuePayment()
        {
            PaymentStatus = TransOrderPaymentStatus.Due;
            AddEvent(OrderEventType.PaymentDue);
        }
    }
}
