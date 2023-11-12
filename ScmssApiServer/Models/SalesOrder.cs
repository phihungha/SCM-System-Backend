namespace ScmssApiServer.Models
{
    public class SalesOrder : Order
    {
        public required string ToLocation { get; set; }

        public ICollection<SalesOrderItem> SalesOrderItems { get; set; }
            = new List<SalesOrderItem>();

        public ICollection<Product> Products { get; set; }
            = new List<Product>();

        public int RetailerId { get; set; }
        public Retailer Retailer { get; set; } = null!;
    }
}
