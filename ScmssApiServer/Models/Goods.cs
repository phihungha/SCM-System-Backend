using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScmssApiServer.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public abstract class Goods : ISoftDeletable
    {
        public DateTime CreateTime { get; set; }
        public string? Description { get; set; }
        public int ExpirationMonth { get; set; }
        public bool HasImage { get; set; }
        public int Id { get; set; }

        /// <summary>
        /// Constants cannot be overridden so use this
        /// to support overriding image folder key name in subclasses.
        /// </summary>
        public abstract string ImageFolderKeyInstance { get; }

        [NotMapped]
        public Uri? ImageUrl => HasImage ? FileHostService.GetUri(ImageFolderKeyInstance, Id) : null;

        public bool IsActive { get; set; }

        public required string Name { get; set; }
        public decimal Price { get; set; }

        public ICollection<ProductionFacility> ProductionFacilities { get; set; }
            = new List<ProductionFacility>();

        public required string Unit { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
