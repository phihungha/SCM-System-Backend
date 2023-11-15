using ScmssApiServer.DomainExceptions;

namespace ScmssApiServer.Models
{
    public abstract class Order<T> : ILifecycle where T : OrderItem
    {
        public int Id { get; set; }

        public ICollection<T> Items { get; set; } = new List<T>();

        public decimal SubTotal { get; set; }
        public double VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Processing;
        public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;

        public string? InvoiceUrl { get; set; }
        public string? ReceiptUrl { get; set; }

        public required string CreateUserId { get; set; }
        public User CreateUser { get; set; } = null!;
        public string? FinishUserId { get; set; }
        public User? FinishUser { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DeliverTime { get; set; }
        public DateTime? FinishTime { get; set; }

        public void AddItem(T item)
        {
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

        public void CompletePayment()
        {
            if (PaymentStatus != OrderPaymentStatus.Due)
            {
                throw new InvalidDomainOperationException(
                    "Cannot complete payment of order if there is no due payment"
                    );
            }
            PaymentStatus = OrderPaymentStatus.Completed;
        }

        public void Complete(string userId)
        {
            if (Status == OrderStatus.Canceled || Status == OrderStatus.Returned)
            {
                throw new InvalidDomainOperationException(
                    "Cannot complete order if it has been canceled or returned"
                    );
            }
            Status = OrderStatus.Completed;
            PaymentStatus = OrderPaymentStatus.Due;
            Finish(userId);
        }

        public void Cancel(string userId)
        {
            if (Status == OrderStatus.Delivered || Status == OrderStatus.Completed)
            {
                throw new InvalidOperationException(
                    "Cannot cancel order after it has been delivered"
                    );
            }
            Status = OrderStatus.Canceled;
            PaymentStatus = OrderPaymentStatus.Canceled;
            Finish(userId);
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
        }

        protected void CalculateTotals()
        {
            SubTotal = Items.Sum(i => i.TotalPrice);
            VatAmount = SubTotal * (decimal)VatRate;
            TotalAmount = SubTotal + VatAmount;
        }

        private void Finish(string userId)
        {
            FinishTime = DateTime.UtcNow;
            FinishUserId = userId;
        }
    }
}
