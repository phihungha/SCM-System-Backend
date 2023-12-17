namespace ScmssApiServer.DTOs
{
    public class WarehouseSupplyItemDto : WarehouseItemDto<WarehouseSupplyItemEventDto>
    {
        public required SupplyDto Supply { get; set; }
        public int SupplyId { get; set; }
    }
}
