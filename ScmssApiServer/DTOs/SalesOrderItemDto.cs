namespace ScmssApiServer.DTOs
{
    public class SalesOrderItemDto : OrderItemDto
    {
        public required GoodsDto Product { get; set; }
    }
}
