using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.Models
{
    /// <summary>
    /// A permission to do something.
    /// </summary>
    public class Permission
    {
        [Key]
        public required string Name { get; set; }

        public required string DisplayName { get; set; }

        public required string Description { get; set; }

        public required IList<Position> Positions { get; set; }
    }
}
