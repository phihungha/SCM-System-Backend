using AutoMapper;
using ScmssApiServer.DTOs;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// A permission to do something.
    /// </summary>
    public class Permission
    {
        public required string Id { get; set; }
        public required string DisplayName { get; set; }
        public required string Description { get; set; }

        public ICollection<Position> Positions { get; } = new List<Position>();

        public ICollection<PositionPermission> PositionPermissions { get; } = new List<PositionPermission>();
    }

    public class PermissionMappingProfile : Profile
    {
        public PermissionMappingProfile()
        {
            CreateMap<Permission, PermissionDto>();
        }
    }
}
