namespace ScmssApiServer.DTOs
{
    public class OrderItemDto
    {
        public int ItemId { get; set; }
        public double Quantity { get; set; }
        public required string Unit { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
