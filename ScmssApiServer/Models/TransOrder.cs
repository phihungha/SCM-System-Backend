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
        /// <summary>
        /// Delivery start location.
        /// </summary>
        public string? FromLocation { get; set; }

        public string? InvoiceUrl { get; set; }

        public TransOrderPaymentStatus PaymentStatus { get; private set; }

        public string? ReceiptUrl { get; set; }

        /// <summary>
        /// Remaining amount to pay.
        /// </summary>
        public decimal RemainingAmount { get; private set; }

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

        /// <summary>
        /// VAT-taxed amount = SubTotal * VatRate
        /// </summary>
        public decimal VatAmount { get; private set; }

        /// <summary>
        /// VAT tax rate from 0 (0%) to 1 (100%).
        /// </summary>
        public double VatRate { get; private set; }

        public override void AddItem(TItem item)
        {
            base.AddItem(item);
            CalculateTotals();
        }

        public TEvent AddManualEvent(TransOrderEventTypeSelection typeSel, string location, string? message)
        {
            if (Status != OrderStatus.Executing)
            {
                throw new InvalidDomainOperationException(
                        "Cannot add manual event when order is not being delivered."
                    );
            }

            TransOrderEventType type;
            switch (typeSel)
            {
                case TransOrderEventTypeSelection.Left:
                    type = TransOrderEventType.Left;
                    break;

                case TransOrderEventTypeSelection.Arrived:
                    type = TransOrderEventType.Arrived;
                    break;

                case TransOrderEventTypeSelection.Interrupted:
                    type = TransOrderEventType.Interrupted;
                    Status = OrderStatus.Interrupted;
                    break;

                default:
                    throw new ArgumentException("Invalid event type");
            }

            return AddEvent(type, location, message);
        }

        public override void Begin(string userId)
        {
            base.Begin(userId);
            PaymentStatus = TransOrderPaymentStatus.Pending;
            AddEvent(TransOrderEventType.Processing);
        }

        public override void Cancel(string userId, string problem)
        {
            base.Cancel(userId, problem);
            PaymentStatus = TransOrderPaymentStatus.Canceled;
            TEvent lastEvent = Events.Last();
            AddEvent(TransOrderEventType.Canceled, lastEvent.Location);
        }

        public override void Complete(string userId)
        {
            base.Complete(userId);
            CreateDuePayment();
            AddEvent(TransOrderEventType.Completed, ToLocation);
        }

        public void CompletePayment(decimal amount)
        {
            if (PaymentStatus != TransOrderPaymentStatus.Due)
            {
                throw new InvalidDomainOperationException(
                        "Cannot complete payment of order if there is no due payment."
                    );
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

        public override void Return(string userId, string problem)
        {
            base.Return(userId, problem);
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
                        "Cannot start delivery of order without start location."
                    );
            }
            base.StartExecution();
            AddEvent(TransOrderEventType.DeliveryStarted, FromLocation);
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
                if (item.IsAutomatic)
                {
                    throw new InvalidDomainOperationException("Cannot edit location of automatic order event.");
                }
                item.Location = location;
            }
            item.Message = message;

            return item;
        }

        protected TEvent AddEvent(TransOrderEventType type, string? location = null, string? message = null)
        {
            if (type == TransOrderEventType.Left || type == TransOrderEventType.Arrived)
            {
                TEvent lastEvent = Events.Last();
                if (type == lastEvent.Type)
                {
                    throw new InvalidDomainOperationException(
                        "Cannot add arrival/left event with the same type as previous arrival/left event."
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

        protected void CalculateTotals()
        {
            SubTotal = Items.Sum(i => i.TotalPrice);
            VatAmount = SubTotal * (decimal)VatRate;
            TotalAmount = SubTotal + VatAmount;
        }

        private void CreateDuePayment()
        {
            PaymentStatus = TransOrderPaymentStatus.Due;
            RemainingAmount = TotalAmount;
            AddEvent(TransOrderEventType.PaymentDue);
        }
    }
}
