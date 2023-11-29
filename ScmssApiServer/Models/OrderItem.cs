namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents an order line.
    /// </summary>
    public abstract class OrderItem
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public double Quantity { get; set; }
        public required string Unit { get; set; }
    }
}
