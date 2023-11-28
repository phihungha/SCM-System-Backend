using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class SalesOrderCreateDto : SalesOrderInputDto
    {
        [Required]
        public IList<TransOrderItemInputDto> Items { get; set; }
            = new List<TransOrderItemInputDto>();

        [Required]
        public int CustomerId { get; set; }
    }
}
