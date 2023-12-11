namespace ScmssApiServer.DTOs
{
    public class ProductInputDto : GoodsInputDto
    {
        public decimal MiscCost { get; set; }
        public double NetWeight { get; set; }

        public ICollection<ProductSupplyCostItemInputDto> SupplyCostItems { get; set; }
            = new List<ProductSupplyCostItemInputDto>();
    }
}
