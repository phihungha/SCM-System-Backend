using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public abstract class TransOrderDto<TItemDto> : OrderDto<TItemDto, TransOrderEventDto>
        where TItemDto : TransOrderItemDto
    {
        public string? FromLocation { get; set; }
        public bool IsPaymentCompleteAllowed { get; set; }
        public bool IsToLocationUpdateAllowed { get; set; }
        public TransOrderPaymentStatus PaymentStatus { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal SubTotal { get; set; }
        public required string ToLocation { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal VatAmount { get; set; }
        public double VatRate { get; set; }
    }
}
