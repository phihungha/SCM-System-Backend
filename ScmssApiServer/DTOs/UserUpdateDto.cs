using ScmssApiServer.Models;
using ScmssApiServer.Validators;
using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class UserUpdateDto
    {
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Password { get; set; }

        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string? Name { get; set; }

        public Gender? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [IdCardNumber]
        public string? IdCardNumber { get; set; }

        public int? PositionId { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }
    }
}
