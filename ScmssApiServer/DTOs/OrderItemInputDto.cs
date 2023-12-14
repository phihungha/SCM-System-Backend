using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class OrderItemInputDto
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        [Range(double.Epsilon, double.MaxValue)]
        public double Quantity { get; set; }
    }
}
