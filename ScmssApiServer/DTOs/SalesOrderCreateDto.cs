using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class SalesOrderCreateDto : OrderCreateDto<OrderItemInputDto>
    {
        [Required]
        public int CustomerId { get; set; }

        public int? ProductionFacilityId { get; set; }
        public string? ToLocation { get; set; }
    }
}
