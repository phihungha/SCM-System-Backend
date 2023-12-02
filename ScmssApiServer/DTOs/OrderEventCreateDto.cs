using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public abstract class OrderEventCreateDto
    {
        [Required]
        public required string Location { get; set; }

        public string? Message { get; set; }
    }
}
