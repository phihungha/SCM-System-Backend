using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public abstract class TransOrderItemDto : OrderItemDto
    {
        public decimal TotalPrice { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
