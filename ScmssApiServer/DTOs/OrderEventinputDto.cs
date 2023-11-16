using ScmssApiServer.Models;
using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class OrderEventInputDto
    {
        [Required]
        public OrderEventTypeSelection Type { get; set; }

        [Required]
        public required string Location { get; set; }

        public string? Message { get; set; }
    }
}
