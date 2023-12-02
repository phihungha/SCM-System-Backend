namespace ScmssApiServer.DTOs
{
    public class ProductInputDto : GoodsInputDto
    {
        public decimal MiscCost { get; set; }
        public double NetWeight { get; set; }

        public ICollection<ProductionSupplyCostItemInputDto> SupplyCostItems { get; set; }
            = new List<ProductionSupplyCostItemInputDto>();
    }
}
