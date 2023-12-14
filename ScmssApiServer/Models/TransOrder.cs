using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents a transacton order such as a sales or purchase order.
    /// </summary>
    /// <typeparam name="TItem">Order line item type</typeparam>
    /// <typeparam name="TEvent">Order event type</typeparam>
    public abstract class TransOrder<TItem, TEvent> : Order<TItem, TEvent>
        where TItem : TransOrderItem
        where TEvent : TransOrderEvent, new()
    {
        private string? fromLocation;

        private string toLocation = "";

        /// <summary>
        /// Delivery start location.
        /// </summary>
        public string? FromLocation
        {
            get => fromLocation;
            set
            {
                if (fromLocation != value && IsExecutionStarted)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot change start location after the order has started delivery."
                        );
                }
                fromLocation = value;
            }
        }

        public TransOrderPaymentStatus PaymentStatus { get; protected set; }

        /// <summary>
        /// Remaining amount to pay.
        /// </summary>
        public decimal RemainingAmount { get; protected set; }

        /// <summary>
        /// Sum of TransOrderItem.TotalPrice.
        /// </summary>
        public decimal SubTotal { get; protected set; }

        /// <summary>
        /// Delivery destination location.
        /// </summary>
        public required string ToLocation
        {
            get => toLocation;
            set
            {
                if (value != toLocation && IsExecutionFinished)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot change destination location after the order has finished delivery."
                        );
                }
                toLocation = value;
            }
        }

        /// <summary>
        /// Total amount to pay = SubTotal + VatAmount
        /// </summary>
        public virtual decimal TotalAmount
        {
            get => SubTotal + VatAmount;
            private set => _ = value;
        }

        /// <summary>
        /// VAT-taxed amount = SubTotal * VatRate
        /// </summary>
        public virtual decimal VatAmount
        {
            get => SubTotal * (decimal)VatRate;
            private set => _ = value;
        }

        /// <summary>
        /// VAT tax rate from 0 (0%) to 1 (100%).
        /// </summary>
        public double VatRate { get; set; }

        public override void AddItems(ICollection<TItem> items)
        {
            base.AddItems(items);
            SubTotal = Items.Sum(i => i.TotalPrice);
        }

        public TEvent AddManualEvent(TransOrderEventTypeSelection typeSel,
                                     string location,
                                     string? message)
        {
            if (!IsExecuting)
            {
                throw new InvalidDomainOperationException(
                        "Cannot add a manual event when the order isn't being delivered."
                    );
            }

            TransOrderEventType type;
            switch (typeSel)
            {
                case TransOrderEventTypeSelection.Left:
                    type = TransOrderEventType.Left;
                    Status = OrderStatus.Executing;
                    break;

                case TransOrderEventTypeSelection.Arrived:
                    type = TransOrderEventType.Arrived;
                    Status = OrderStatus.Executing;
                    break;

                case TransOrderEventTypeSelection.Interrupted:
                    type = TransOrderEventType.Interrupted;
                    Status = OrderStatus.Interrupted;
                    break;

                default:
                    throw new ArgumentException("Invalid event type.");
            }

            return AddEvent(type, location, message);
        }

        public override void Begin(User user)
        {
            base.Begin(user);
            PaymentStatus = TransOrderPaymentStatus.Pending;
            AddEvent(TransOrderEventType.Processing);
        }

        public override void Cancel(User user, string problem)
        {
            base.Cancel(user, problem);
            PaymentStatus = TransOrderPaymentStatus.Canceled;
            TEvent lastEvent = Events.Last();
            AddEvent(TransOrderEventType.Canceled, lastEvent.Location);
        }

        public override void Complete(User user)
        {
            base.Complete(user);
            CreateDuePayment();
            AddEvent(TransOrderEventType.Completed, ToLocation);
        }

        public void CompletePayment(decimal amount)
        {
            if (PaymentStatus != TransOrderPaymentStatus.Due)
            {
                throw new InvalidDomainOperationException(
                        "Cannot complete order payment if there is no due payment."
                    );
            }

            if (amount > RemainingAmount)
            {
                throw new InvalidDomainOperationException("Cannot pay more than remaining amount.");
            }

            RemainingAmount = RemainingAmount - amount;
            if (RemainingAmount == 0)
            {
                PaymentStatus = TransOrderPaymentStatus.Completed;
                AddEvent(TransOrderEventType.PaymentCompleted);
            }
        }

        /// <summary>
        /// Finish order delivery.
        /// </summary>
        public override void FinishExecution()
        {
            base.FinishExecution();
            AddEvent(TransOrderEventType.Delivered, ToLocation);
        }

        public override void Return(User user, string problem)
        {
            base.Return(user, problem);
            PaymentStatus = TransOrderPaymentStatus.Canceled;
            AddEvent(TransOrderEventType.Returned, ToLocation);
        }

        /// <summary>
        /// Start order delivery.
        /// </summary>
        public override void StartExecution()
        {
            if (FromLocation == null)
            {
                throw new InvalidDomainOperationException(
                        "Cannot start order delivery without start location."
                    );
            }
            base.StartExecution();
            AddEvent(TransOrderEventType.DeliveryStarted, FromLocation);
        }

        protected TEvent AddEvent(TransOrderEventType type, string? location = null, string? message = null)
        {
            if (type == TransOrderEventType.Left || type == TransOrderEventType.Arrived)
            {
                TEvent lastEvent = Events.Last();
                if (type == lastEvent.Type)
                {
                    throw new InvalidDomainOperationException(
                        "Cannot add an arrival/left event with the same type " +
                        "as the previous arrival/left event."
                    );
                }
            }

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

        private void CreateDuePayment()
        {
            PaymentStatus = TransOrderPaymentStatus.Due;
            RemainingAmount = TotalAmount;
            AddEvent(TransOrderEventType.PaymentDue);
        }
    }
}
