﻿using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    public abstract class Order<TItem, TEvent> : ILifecycle
        where TItem : OrderItem
        where TEvent : OrderEvent, new()
    {
        public int Id { get; set; }

        public ICollection<TItem> Items { get; } = new List<TItem>();

        public decimal SubTotal { get; private set; }
        public double VatRate { get; private set; }
        public decimal VatAmount { get; private set; }
        public decimal TotalAmount { get; private set; }

        public string? FromLocation { get; set; }
        public required string ToLocation { get; set; }

        public OrderStatus Status { get; private set; }
        public OrderPaymentStatus PaymentStatus { get; private set; }

        public string? InvoiceUrl { get; set; }
        public string? ReceiptUrl { get; set; }

        public ICollection<TEvent> Events { get; } = new List<TEvent>();

        public required string CreateUserId { get; set; }
        public User CreateUser { get; set; } = null!;
        public string? FinishUserId { get; private set; }
        public User? FinishUser { get; private set; }

        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DeliverTime { get; protected set; }
        public DateTime? FinishTime { get; private set; }
        public bool Finished { get => FinishTime != null; }

        public void AddItem(TItem item)
        {
            if (Status != OrderStatus.Processing)
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
            if (Status != OrderStatus.Delivering)
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

        public void Start(string userId)
        {
            if (Id != 0)
            {
                throw new InvalidDomainOperationException("Cannot start an already created order");
            }

            Status = OrderStatus.Processing;
            PaymentStatus = OrderPaymentStatus.Pending;
            CreateUserId = userId;
            AddEvent(OrderEventType.Processing);
        }

        public virtual void StartDelivery()
        {
            if (Status != OrderStatus.Processing)
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
            Status = OrderStatus.Delivering;
            AddEvent(OrderEventType.DeliveryStarted, FromLocation);
        }

        public void FinishDelivery()
        {
            if (Status != OrderStatus.Delivering)
            {
                throw new InvalidDomainOperationException(
                        "Cannot finish delivery of order if it is not being delivered."
                    );
            }
            Status = OrderStatus.Delivered;
            DeliverTime = DateTime.UtcNow;
            AddEvent(OrderEventType.Delivered, ToLocation);
        }

        public void Complete(string userId)
        {
            if (Status != OrderStatus.Delivered)
            {
                throw new InvalidDomainOperationException(
                        "Cannot complete order if it hasn't finished delivery."
                    );
            }
            Status = OrderStatus.Completed;
            AddEvent(OrderEventType.Completed, ToLocation);
            CreateDuePayment();
            Finish(userId);
        }

        public void Cancel(string userId)
        {
            if (Status == OrderStatus.Delivered || Status == OrderStatus.Completed)
            {
                throw new InvalidOperationException(
                        "Cannot cancel order after it has been delivered."
                    );
            }
            Status = OrderStatus.Canceled;
            PaymentStatus = OrderPaymentStatus.Canceled;
            Finish(userId);

            TEvent lastEvent = Events.Last();
            AddEvent(OrderEventType.Canceled, lastEvent.Location);
        }

        public void Return(string userId)
        {
            if (Status != OrderStatus.Delivered)
            {
                throw new InvalidOperationException(
                        "Cannot return order if it has been completed or hasn't finished delivery."
                    );
            }
            Status = OrderStatus.Returned;
            PaymentStatus = OrderPaymentStatus.Canceled;
            Finish(userId);
            AddEvent(OrderEventType.Returned, ToLocation);
        }

        public void CompletePayment()
        {
            if (PaymentStatus != OrderPaymentStatus.Due)
            {
                throw new InvalidDomainOperationException(
                        "Cannot complete payment of order if there is no due payment."
                    );
            }
            PaymentStatus = OrderPaymentStatus.Completed;
            AddEvent(OrderEventType.PaymentCompleted);
        }

        private void CreateDuePayment()
        {
            PaymentStatus = OrderPaymentStatus.Due;
            AddEvent(OrderEventType.PaymentDue);
        }

        protected void CalculateTotals()
        {
            SubTotal = Items.Sum(i => i.TotalPrice);
            VatAmount = SubTotal * (decimal)VatRate;
            TotalAmount = SubTotal + VatAmount;
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

        public void Finish(string userId)
        {
            if (FinishTime != null)
            {
                throw new InvalidDomainOperationException("Order is finished");
            }

            FinishTime = DateTime.UtcNow;
            FinishUserId = userId;
        }
    }
}
