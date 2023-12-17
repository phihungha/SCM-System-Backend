using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class WarehouseItemInputDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Quantity { get; set; }
    }
}
