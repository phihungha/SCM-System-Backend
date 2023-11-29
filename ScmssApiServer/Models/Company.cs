namespace ScmssApiServer.Models
{
    public class Company : IUpdateTrackable
    {
        public string? ContactPerson { get; set; }
        public DateTime CreateTime { get; set; }
        public required string DefaultLocation { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public required string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
