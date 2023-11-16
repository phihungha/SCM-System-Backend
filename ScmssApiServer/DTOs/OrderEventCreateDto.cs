using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class OrderEventCreateDto : OrderEventInputDto
    {
        [Required]
        public OrderEventTypeSelection Type { get; set; }

        [Required]
        public required string Location { get; set; }
    }
}
