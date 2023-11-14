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

        public OrderStatus Status { get; set; }
        public OrderPaymentStatus PaymentStatus { get; set; }

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
                throw new InvalidOperationException("An order item with this ID already exists");
            }
            Items.Add(item);
            CalculateTotals();
        }

        public void CompletePayment(string userId)
        {
            if (Status == OrderStatus.Canceled || Status == OrderStatus.Returned)
            {
                throw new InvalidOperationException(
                    "Cannot complete payment of order after it has been canceled or returned"
                    );
            }
            else if (PaymentStatus != OrderPaymentStatus.Due)
            {
                throw new InvalidOperationException(
                    "Cannot complete payment of order if there is no due payment"
                    );
            }

            PaymentStatus = OrderPaymentStatus.Completed;
            Finish(userId);
        }

        public void Cancel(string userId)
        {
            if (Status == OrderStatus.Delivered)
            {
                throw new InvalidOperationException(
                    "Cannot cancel order after it has been delivered"
                    );
            }
            else if (PaymentStatus == OrderPaymentStatus.Completed)
            {
                throw new InvalidOperationException(
                    "Cannot cancel order after it has been paid"
                    );
            }

            Status = OrderStatus.Canceled;
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
