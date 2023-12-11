using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class ProductionFacilityInputDto : ISoftDeletableInputDto
    {
        public string? Description { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public bool? IsActive { get; set; }

        [Required]
        public required string Location { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }
    }
}
