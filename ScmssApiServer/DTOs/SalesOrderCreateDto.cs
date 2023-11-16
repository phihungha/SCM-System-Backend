using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class SalesOrderCreateDto : SalesOrderInputDto
    {
        [Required]
        public IList<OrderItemInputDto> Items { get; set; }
            = new List<OrderItemInputDto>();

        [Required]
        public int CustomerId { get; set; }
    }
}
