using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class SalesOrderInputDto
    {
        public string? ToLocation { get; set; }
        public int? ProductionFacilityId { get; set; }

        [Required]
        public IList<OrderItemInputDto> Items { get; set; }
            = new List<OrderItemInputDto>();

        public SalesOrderStatusSelection? Status { get; set; }

        public bool? PaymentCompleted { get; set; }
    }
}
