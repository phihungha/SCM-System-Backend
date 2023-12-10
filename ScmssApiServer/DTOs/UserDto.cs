using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class UserDto : ISoftDeletableDto
    {
        public required string Id { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public required string Name { get; set; }

        public required Gender Gender { get; set; }

        public required DateTime DateOfBirth { get; set; }

        public required string IdCardNumber { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public required DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
