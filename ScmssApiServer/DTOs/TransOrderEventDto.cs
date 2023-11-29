using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class TransOrderEventDto : OrderEventDto
    {
        public TransOrderEventType Type { get; set; }
    }
}
