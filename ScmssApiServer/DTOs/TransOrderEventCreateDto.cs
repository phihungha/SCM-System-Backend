using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class TransOrderEventCreateDto : OrderEventCreateDto
    {
        [Required]
        public TransOrderEventTypeOption Type { get; set; }
    }
}
