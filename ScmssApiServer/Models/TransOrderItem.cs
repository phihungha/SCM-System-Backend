namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents an order line in a transaction order.
    /// </summary>
    public abstract class TransOrderItem : OrderItem
    {
        public decimal TotalPrice
        {
            get => UnitPrice * (decimal)Quantity;
            private set => _ = value;
        }

        public decimal UnitPrice { get; set; }
    }
}
