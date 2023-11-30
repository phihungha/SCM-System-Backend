using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class SalesOrderCreateDto : OrderCreateDto
    {
        [Required]
        public int CustomerId { get; set; }

        public int? ProductionFacilityId { get; set; }
        public string? ToLocation { get; set; }
    }
}
