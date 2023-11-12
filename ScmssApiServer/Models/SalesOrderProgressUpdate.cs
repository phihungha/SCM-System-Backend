namespace ScmssApiServer.Models
{
    public class SalesOrderProgressUpdate : OrderProgressUpdate
    {
        public int SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; } = null!;
    }
}
