namespace ScmssApiServer.DTOs
{
    public class SalesOrderUpdateDto : TransOrderUpdateDto<OrderItemInputDto>
    {
        public int? ProductionFacilityId { get; set; }
        public string? ToLocation { get; set; }
    }
}
