using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderItemDiscountInputDto
    {
        public decimal Discount { get; set; }

        [Required]
        public int ItemId { get; set; }
    }
}
