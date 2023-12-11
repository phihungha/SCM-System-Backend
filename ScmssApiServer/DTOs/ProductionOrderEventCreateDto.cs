using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class ProductionOrderEventCreateDto : OrderEventCreateDto
    {
        [Required]
        public ProductionOrderEventTypeOption Type { get; set; }
    }
}
