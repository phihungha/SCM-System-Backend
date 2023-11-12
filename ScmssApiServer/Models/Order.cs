namespace ScmssApiServer.Models
{
    public abstract class Order : ILifecycle
    {
        public int Id { get; set; }

        public decimal SubTotal { get; set; }
        public double VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }
        public OrderPaymentStatus PaymentStatus { get; set; }

        public required string CreateUserId { get; set; }
        public User CreateUser { get; set; } = null!;
        public string? FinishUserId { get; set; }
        public User? FinishUser { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DeliverTime { get; set; }
        public DateTime? FinishTime { get; set; }
    }
}
