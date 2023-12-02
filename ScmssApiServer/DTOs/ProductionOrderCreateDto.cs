using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class ProductionOrderCreateDto : OrderCreateDto
    {
        [Required]
        public int ProductionFacilityId { get; set; }
    }
}
