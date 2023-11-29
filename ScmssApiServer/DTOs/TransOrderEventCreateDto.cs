using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class TransOrderEventCreateDto : OrderEventInputDto
    {
        [Required]
        public TransOrderEventTypeSelection Type { get; set; }

        [Required]
        public required string Location { get; set; }
    }
}
