using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class SupplyInputDto : GoodsInputDto
    {
        [Required]
        public int VendorId { get; set; }
    }
}
