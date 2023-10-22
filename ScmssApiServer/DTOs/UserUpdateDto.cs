using ScmssApiServer.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.DTOs
{
    public class UserUpdateDto
    {
        [StringLength(maximumLength: 20, MinimumLength = 5)]
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

        [StringLength(maximumLength: 12, MinimumLength = 12)]
        [Column(TypeName = "char(12)")]
        public string? IdCardNumber { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }
    }
}
