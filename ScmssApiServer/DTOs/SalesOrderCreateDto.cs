using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class SalesOrderCreateDto : SalesOrderInputDto
    {
        [Required]
        public int CustomerId { get; set; }
    }
}
