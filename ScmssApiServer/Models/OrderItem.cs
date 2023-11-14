namespace ScmssApiServer.Models
{
    public abstract class OrderItem
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }

        public double Quantity { get; set; }
        public required string Unit { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice
        {
            get => UnitPrice * (decimal)Quantity;
            set => _ = value;
        }
    }
}
