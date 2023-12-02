namespace ScmssApiServer.DTOs
{
    public class SalesOrderUpdateDto : TransOrderUpdateDto
    {
        public int? ProductionFacilityId { get; set; }
        public string? ToLocation { get; set; }
    }
}
