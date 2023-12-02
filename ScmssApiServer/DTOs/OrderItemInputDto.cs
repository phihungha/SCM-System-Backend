using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class OrderItemInputDto
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        public double Quantity { get; set; }
    }
}
