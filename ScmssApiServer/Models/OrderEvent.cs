namespace ScmssApiServer.Models
{
    public abstract class OrderEvent
    {
        public int Id { get; set; }
        public string? Location { get; set; }
        public string? Message { get; set; }
        public DateTime Time { get; set; }
    }
}
