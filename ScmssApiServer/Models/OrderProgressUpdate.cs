namespace ScmssApiServer.Models
{
    public abstract class OrderProgressUpdate
    {
        public int Id { get; set; }
        public OrderProgressUpdateType Type { get; set; }
        public DateTime Time { get; set; }
        public required string Location { get; set; }
        public string? Message { get; set; }
    }
}
