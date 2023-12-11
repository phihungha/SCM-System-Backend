namespace ScmssApiServer.DTOs
{
    public class SupplyDto : GoodsDto
    {
        public CompanyDto Vendor { get; set; } = null!;
        public int VendorId { get; set; }
    }
}
