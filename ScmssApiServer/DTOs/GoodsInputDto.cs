using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class GoodsInputDto : ISoftDeletableInputDto
    {
        public string? Description { get; set; }

        [Required]
        public int ExpirationMonth { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public required string Unit { get; set; }
    }
}
