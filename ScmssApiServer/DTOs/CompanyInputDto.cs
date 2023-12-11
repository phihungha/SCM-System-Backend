using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class CompanyInputDto : ISoftDeletableInputDto
    {
        public string? ContactPerson { get; set; }

        [Required]
        public required string DefaultLocation { get; set; }

        public string? Description { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public bool? IsActive { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }
    }
}
