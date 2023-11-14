namespace ScmssApiServer.Models
{
    public class SalesOrderItem : OrderItem
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; } = null!;
    }
}
