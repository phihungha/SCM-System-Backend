namespace ScmssApiServer.Models
{
    /// <summary>
    /// Represents a generic order event.
    /// </summary>
    public abstract class OrderEvent
    {
        public int Id { get; set; }
        public abstract bool IsAutomatic { get; }
        public string? Location { get; set; }
        public string? Message { get; set; }
        public DateTime Time { get; set; }
    }
}
