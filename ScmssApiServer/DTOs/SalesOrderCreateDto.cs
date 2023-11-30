using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class SalesOrderCreateDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public IList<OrderItemInputDto> Items { get; set; }
            = new List<OrderItemInputDto>();

        public int? ProductionFacilityId { get; set; }
        public string? ToLocation { get; set; }
    }
}
