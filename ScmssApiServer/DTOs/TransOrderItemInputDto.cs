using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class TransOrderItemInputDto
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        public double Quantity { get; set; }
    }
}
