using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class ProductionOrderEventDto : OrderEventDto
    {
        public ProductionOrderEventType Type { get; set; }
    }
}
