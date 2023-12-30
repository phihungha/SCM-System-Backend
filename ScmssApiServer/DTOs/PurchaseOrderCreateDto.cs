using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderCreateDto
    {
        public decimal? AdditionalDiscount { get; set; }
        public string? FromLocation { get; set; }

        public ICollection<PurchaseOrderItemDiscountInputDto>? Items { get; set; }

        [Required]
        public int PurchaseRequisitionId { get; set; }
    }
}
