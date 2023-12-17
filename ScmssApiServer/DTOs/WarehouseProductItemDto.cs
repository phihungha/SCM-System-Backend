namespace ScmssApiServer.DTOs
{
    public class WarehouseProductItemDto : WarehouseItemDto<WarehouseProductItemEventDto>
    {
        public required ProductDto Product { get; set; }
        public int ProductId { get; set; }
    }
}
