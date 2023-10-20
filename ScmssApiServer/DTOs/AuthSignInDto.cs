using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class AuthSignInDto
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
