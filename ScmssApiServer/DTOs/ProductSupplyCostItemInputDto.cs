using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class ProductSupplyCostItemInputDto
    {
        [Required]
        [Range(double.Epsilon, double.MaxValue)]
        public double Quantity { get; set; }

        [Required]
        public int SupplyId { get; set; }
    }
}
