using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderItemUpdateDto
    {
        public decimal Discount { get; set; }

        [Required]
        public int ItemId { get; set; }
    }
}
