using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class TransOrderEventCreateDto : OrderEventCreateDto
    {
        [Required]
        public TransOrderEventTypeSelection Type { get; set; }
    }
}
