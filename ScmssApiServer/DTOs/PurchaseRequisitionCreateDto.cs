using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class PurchaseRequisitionCreateDto
    {
        [Required]
        public required ICollection<OrderItemInputDto> Items { get; set; }

        [Required]
        public int VendorId { get; set; }
    }
}
