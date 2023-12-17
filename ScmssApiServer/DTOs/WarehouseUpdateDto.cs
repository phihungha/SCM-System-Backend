namespace ScmssApiServer.DTOs
{
    public class WarehouseUpdateDto<T> where T : WarehouseItemInputDto
    {
        public ICollection<T> Items { get; set; } = new List<T>();
    }
}
