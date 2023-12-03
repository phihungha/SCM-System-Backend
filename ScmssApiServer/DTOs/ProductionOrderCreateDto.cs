using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class ProductionOrderCreateDto : OrderCreateDto<OrderItemInputDto>
    {
        [Required]
        public int ProductionFacilityId { get; set; }
    }
}
