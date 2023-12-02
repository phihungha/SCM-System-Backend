namespace ScmssApiServer.DTOs
{
    public class SupplyDto : GoodsDto
    {
        public int VendorId { get; set; }
        public CompanyDto Vendor { get; set; } = null!;
    }
}
