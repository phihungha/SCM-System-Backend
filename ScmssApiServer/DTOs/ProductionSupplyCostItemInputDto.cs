using ScmssApiServer.Models;
using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class ProductionSupplyCostItemInputDto
    {
        [Required]
        public int SupplyId { get; set; }

        [Required]
        public double Quantity { get; set; }
    }
}
