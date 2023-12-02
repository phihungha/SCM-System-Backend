namespace ScmssApiServer.DTOs
{
    public class SalesOrderItemDto : TransOrderItemDto
    {
        public required GoodsDto Product { get; set; }
    }
}
