using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public abstract class OrderCreateDto
    {
        [Required]
        public IList<OrderItemInputDto> Items { get; set; }
            = new List<OrderItemInputDto>();
    }
}
