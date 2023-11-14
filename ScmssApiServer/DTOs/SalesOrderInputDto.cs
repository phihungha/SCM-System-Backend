using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class SalesOrderInputDto
    {
        [Required]
        public required string ToLocation { get; set; }

        [Required]
        public IList<OrderItemInputDto> Items { get; set; }
            = new List<OrderItemInputDto>();
    }
}
