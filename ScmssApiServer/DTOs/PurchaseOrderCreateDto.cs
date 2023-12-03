using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class PurchaseOrderCreateDto
    {
        public string? FromLocation { get; set; }

        [Required]
        public int PurchaseRequisitionId { get; set; }
    }
}
