namespace ScmssApiServer.Models
{
    public class Position : Entity, IUpdateTrackable
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required IList<Permission> Permissions { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
