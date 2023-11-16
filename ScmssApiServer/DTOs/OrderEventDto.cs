using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class OrderEventDto
    {
        public int Id { get; set; }
        public OrderEventType Type { get; set; }
        public DateTime Time { get; set; }
        public string? Location { get; set; }
        public string? Message { get; set; }
    }
}
