namespace ScmssApiServer.Models
{
    /// <summary>
    /// The position a user holds in the company.
    /// </summary>
    public class Position : EntityWithId, IUpdateTrackable
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

        public ICollection<Permission> Permissions { get; } = new List<Permission>();
        public ICollection<PositionPermission> PositionPermissions { get; } = new List<PositionPermission>();

        public ICollection<User> Users { get; } = new List<User>();

        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
