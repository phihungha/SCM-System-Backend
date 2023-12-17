namespace ScmssApiServer.DTOs
{
    public abstract class WarehouseItemEventDto
    {
        public double Change { get; set; }
        public double Quantity { get; set; }
        public DateTime Time { get; set; }
    }
}
