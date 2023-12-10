using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderItemInputDto
    {
        public decimal Discount { get; set; }

        [Required]
        public int ItemId { get; set; }
    }
}
