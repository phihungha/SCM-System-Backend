using ScmssApiServer.Models;

namespace ScmssApiServer.DTOs
{
    public class UserDto : ISoftDeletableDto
    {
        public string? Address { get; set; }
        public required DateTime CreateTime { get; set; }
        public required string DateOfBirth { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public required Gender Gender { get; set; }
        public required string Id { get; set; }
        public required string IdCardNumber { get; set; }
        public string? ImageName { get; set; }
        public Uri? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public required string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public ProductionFacilityDto? ProductionFacility { get; set; }
        public int? ProductionFacilityId { get; set; }
        public ICollection<string> Roles { get; set; } = new List<string>();
        public DateTime? UpdateTime { get; set; }
        public string? UserName { get; set; }
    }
}
