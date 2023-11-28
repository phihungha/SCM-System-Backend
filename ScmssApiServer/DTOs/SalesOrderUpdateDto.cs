namespace ScmssApiServer.DTOs
{
    public class SalesOrderUpdateDto : SalesOrderInputDto
    {
        public IList<TransOrderItemInputDto>? Items { get; set; }

        public TransOrderStatusSelection? Status { get; set; }
        public bool? PaymentCompleted { get; set; }
    }
}
