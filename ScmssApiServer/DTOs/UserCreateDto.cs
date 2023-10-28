using ScmssApiServer.Models;
using System.ComponentModel.DataAnnotations;
using ScmssApiServer.Validators;

namespace ScmssApiServer.DTOs
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 5)]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required string Phone { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public required string Name { get; set; }

        [Required]
        public required Gender Gender { get; set; }

        [Required]
        public required DateTime DateOfBirth { get; set; }

        [Required]
        [IdCardNumber]
        public required string IdCardNumber { get; set; }

        [Required]
        public int PostitionId { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }
    }
}
