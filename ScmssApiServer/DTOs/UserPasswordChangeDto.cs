using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class UserPasswordChangeDto
    {
        [Required]
        public required string CurrentPassword { get; set; }

        [Required]
        public required string NewPassword { get; set; }
    }
}
