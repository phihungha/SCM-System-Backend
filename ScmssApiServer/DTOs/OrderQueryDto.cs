using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class OrderQueryDto<T> where T : Enum
    {
        public T? SearchCriteria { get; set; }
        public string? SearchTerm { get; set; }
        public ICollection<OrderStatus>? Status { get; set; }
    }
}
