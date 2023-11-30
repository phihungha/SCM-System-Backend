using ScmssApiServer.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.Models
{
    public class ProductionOrderCreateDto : OrderCreateDto
    {
        [Required]
        public int ProductionFacilityId { get; set; }
    }
}
