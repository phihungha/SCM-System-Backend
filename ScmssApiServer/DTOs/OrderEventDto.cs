namespace ScmssApiServer.DTOs
{
    public abstract class OrderEventDto
    {
        public int Id { get; set; }
        public string? Location { get; set; }
        public string? Message { get; set; }
        public DateTime Time { get; set; }
        public bool IsAutomatic { get; set; }
    }
}
