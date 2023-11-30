namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents a production order line.
    /// </summary>
    public class ProductionOrderItem : OrderItem
    {
        public ProductionOrder ProductionOrder { get; set; } = null!;
        public Product Product { get; set; } = null!;

        /// <summary>
        /// Total production cost of this item = UnitCost * Quantity
        /// </summary>
        public decimal TotalCost
        {
            get => UnitCost * (decimal)Quantity;
            private set => _ = value;
        }

        /// <summary>
        /// Total value of this item = UnitValue * Quantity
        /// </summary>
        public decimal TotalValue
        {
            get => UnitValue * (decimal)Quantity;
            private set => _ = value;
        }

        /// <summary>
        /// Unit cost of this item = Product.ProductionCost
        /// </summary>
        public decimal UnitCost { get; set; }

        /// <summary>
        /// Unit value of this item = Product.Price
        /// </summary>
        public decimal UnitValue { get; set; }
    }
}
