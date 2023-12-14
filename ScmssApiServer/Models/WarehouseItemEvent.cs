namespace ScmssApiServer.Models
{
    public abstract class WarehouseItemEvent
    {
        public double Change { get; set; }
        public double Quantity { get; set; }
        public DateTime Time { get; set; }
    }
}
