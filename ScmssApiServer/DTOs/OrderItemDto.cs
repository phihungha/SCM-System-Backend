namespace ScmssApiServer.Models
{
    public abstract class OrderItemDto
    {
        public int ItemId { get; set; }
        public double Quantity { get; set; }
        public required string Unit { get; set; }
    }
}
