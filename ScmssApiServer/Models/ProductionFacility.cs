namespace ScmssApiServer.Models
{
    public class ProductionFacility
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Location { get; set; }

        public ICollection<ProductionOrder> ProductionOrder { get; set; }
            = new List<ProductionOrder>();
    }
}
