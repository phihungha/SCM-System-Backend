using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class TransOrderQueryDto<T> : OrderQueryDto<T> where T : Enum
    {
        public ICollection<TransOrderPaymentStatus>? PaymentStatus { get; set; }
    }
}
