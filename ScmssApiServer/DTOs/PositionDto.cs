namespace ScmssApiServer.DTOs
{
    public class PositionDto : DtoWithId
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

        public IList<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();

        public bool IsActive { get; set; }
        public required DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
