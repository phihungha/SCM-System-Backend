using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public abstract class OrderDto
    {
        public int Id { get; set; }

        public decimal SubTotal { get; set; }
        public double VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }
        public OrderPaymentStatus PaymentStatus { get; set; }

        public string? InvoiceUrl { get; set; }
        public string? ReceiptUrl { get; set; }

        public ICollection<OrderEvent> Events { get; set; } = new List<OrderEvent>();

        public required string CreateUserId { get; set; }
        public UserDto CreateUser { get; set; } = null!;
        public string? FinishUserId { get; set; }
        public UserDto? FinishUser { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DeliverTime { get; set; }
        public DateTime? FinishTime { get; set; }
    }
}
