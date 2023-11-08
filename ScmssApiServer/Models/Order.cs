namespace ScmssApiServer.Models
{
    public abstract class Order : IUpdateTrackable
    {
        public int Id { get; set; }

        public decimal SubTotal { get; set; }
        public double VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }
        public OrderPaymentStatus PaymentStatus { get; set; }

        public int CreatedUserId { get; set; }
        public User CreatedUser { get; set; } = null!;
        public int? FinishedUserId { get; set; }
        public User? FinishedUser { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime? FinishedTime { get; set; }
    }
}
