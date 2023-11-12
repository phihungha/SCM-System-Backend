namespace ScmssApiServer.Models
{
    public class Product : Goods
    {
        public decimal ProductionCost { get; set; }

        public IList<SalesOrder> SalesOrders { get; set; }
            = new List<SalesOrder>();

        public IList<SalesOrderItem> SalesOrderItems { get; set; }
            = new List<SalesOrderItem>();
    }
}
