using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class UserCreateDto : UserInputDto
    {
        [Required]
        public required string Password { get; set; }
    }
}
