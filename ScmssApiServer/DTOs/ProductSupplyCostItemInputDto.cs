using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class ProductSupplyCostItemInputDto
    {
        [Required]
        public double Quantity { get; set; }

        [Required]
        public int SupplyId { get; set; }
    }
}
