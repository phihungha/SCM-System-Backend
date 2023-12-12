using ScmssApiServer.Models;
using ScmssApiServer.Validators;
using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class UserInputDto : ISoftDeletableInputDto
    {
        public string? Address { get; set; }

        [Required]
        public required string DateOfBirth { get; set; }

        public string? Description { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required Gender Gender { get; set; }

        [Required]
        [IdCardNumber]
        public required string IdCardNumber { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public required string Name { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        [Required]
        public ICollection<string> Roles { get; set; } = new List<string>();

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 5)]
        public required string UserName { get; set; }
    }
}
