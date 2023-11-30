namespace ScmssApiServer.DTOs
{
    public abstract class TransOrderUpdateDto : OrderUpdateDto
    {
        public decimal? PaymentAmount { get; set; }
        public bool? PaymentCompleted { get; set; }
    }
}
