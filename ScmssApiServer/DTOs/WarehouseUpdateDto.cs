namespace ScmssApiServer.DTOs
{
    public class WarehouseUpdateDto
    {
        public ICollection<WarehouseItemInputDto> Items { get; set; }
            = new List<WarehouseItemInputDto>();
    }
}
