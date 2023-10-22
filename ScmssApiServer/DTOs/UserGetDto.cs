using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class UserGetDto : IUpdateTrackableDto
    {
        public required string Id { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public required string Name { get; set; }

        public required Gender Gender { get; set; }

        public required DateTime DateOfBirth { get; set; }

        public required string IdCardNumber { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }
    }
}
