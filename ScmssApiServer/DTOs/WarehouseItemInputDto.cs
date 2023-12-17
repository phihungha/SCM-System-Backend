using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public abstract class WarehouseItemInputDto
    {
        [Required]
        [Range(0, double.MaxValue)]
        public double Quantity { get; set; }
    }
}
