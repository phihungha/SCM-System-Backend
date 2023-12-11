using Microsoft.EntityFrameworkCore;

namespace ScmssApiServer.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Company : ISoftDeletable
    {
        public string? ContactPerson { get; set; }
        public DateTime CreateTime { get; set; }
        public required string DefaultLocation { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
