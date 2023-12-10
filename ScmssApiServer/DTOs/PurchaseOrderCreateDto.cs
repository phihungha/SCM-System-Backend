using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderCreateDto : OrderCreateDto<PurchaseOrderItemInputDto>
    {
        public decimal? AdditionalDiscount { get; set; }
        public string? FromLocation { get; set; }

        [Required]
        public int PurchaseRequisitionId { get; set; }
    }
}
