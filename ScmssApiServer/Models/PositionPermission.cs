namespace ScmssApiServer.Models
{
    /// <summary>
    /// Join entity for many-to-many between Position and Permission.
    /// </summary>
    public class PositionPermission
    {
        public required string PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
        public int PositionId { get; set; }
        public Position Position { get; set; } = null!;
    }
}
