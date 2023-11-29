namespace ScmssApiServer.DTOs
{
    public abstract class TransOrderItemDto
    {
        public int ItemId { get; set; }
        public double Quantity { get; set; }
        public required string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
