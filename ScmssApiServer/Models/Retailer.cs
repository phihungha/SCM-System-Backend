namespace ScmssApiServer.Models
{
    public class Retailer : Company
    {
        public IList<SalesOrder> SalesOrders { get; set; }
            = new List<SalesOrder>();
    }
}
