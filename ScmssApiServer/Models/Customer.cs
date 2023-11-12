namespace ScmssApiServer.Models
{
    public class Customer : Company
    {
        public IList<SalesOrder> SalesOrders { get; set; }
            = new List<SalesOrder>();
    }
}
