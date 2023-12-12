using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class GoodsInputDto : ISoftDeletableInputDto
    {
        public string? Description { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public int ExpirationMonth { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [Range(double.Epsilon, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public required string Unit { get; set; }
    }
}
