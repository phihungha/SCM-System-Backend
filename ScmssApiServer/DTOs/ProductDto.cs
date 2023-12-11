namespace ScmssApiServer.DTOs
{
    public class ProductDto : GoodsDto
    {
        public decimal MiscCost { get; set; }
        public double NetWeight { get; set; }
        public decimal Cost { get; set; }
        public decimal Profit { get; set; }
        public decimal SupplyCost { get; set; }

        public ICollection<ProductSupplyCostItemDto> SupplyCostItems { get; set; }
                    = new List<ProductSupplyCostItemDto>();
    }
}
