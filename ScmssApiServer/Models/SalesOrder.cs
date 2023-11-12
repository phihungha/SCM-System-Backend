namespace ScmssApiServer.Models
{
    public class SalesOrder : Order
    {
        public required string ToLocation { get; set; }

        public ICollection<SalesOrderItem> Items { get; set; }
            = new List<SalesOrderItem>();

        public ICollection<Product> Products { get; set; }
            = new List<Product>();

        public ICollection<SalesOrderProgressUpdate> ProgressUpdates { get; set; }
            = new List<SalesOrderProgressUpdate>();

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public int ProductionFacilityId { get; set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;
    }
}
