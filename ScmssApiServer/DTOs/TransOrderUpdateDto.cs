namespace ScmssApiServer.DTOs
{
    public abstract class TransOrderUpdateDto<TItemDto> : OrderUpdateDto<TItemDto>
    {
        public decimal? PaymentAmount { get; set; }
    }
}
