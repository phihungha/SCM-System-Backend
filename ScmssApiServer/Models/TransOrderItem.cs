namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents an order line in a transaction order.
    /// </summary>
    public abstract class TransOrderItem
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }

        public double Quantity { get; set; }
        public required string Unit { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice
        {
            get => UnitPrice * (decimal)Quantity;
            private set => _ = value;
        }
    }
}
