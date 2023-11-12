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

        public ICollection<WarehouseSupplyItem> WarehouseSupplyItems { get; }
            = new List<WarehouseSupplyItem>();

        public ICollection<Supply> WarehouseSupplies { get; } = new List<Supply>();

        public ICollection<WarehouseProductItem> WarehouseProductItems { get; }
            = new List<WarehouseProductItem>();

        public ICollection<Product> WarehouseProducts { get; } = new List<Product>();
    }
}
