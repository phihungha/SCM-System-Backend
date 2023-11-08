namespace ScmssApiServer.Models
{
    public class SalesOrder : Order
    {
        public required string Location { get; set; }

        public int RetailerId { get; set; }
        public Retailer Retailer { get; set; } = null!;
    }
}
