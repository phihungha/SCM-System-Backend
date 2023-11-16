namespace ScmssApiServer.DTOs
{
    public abstract class SalesOrderInputDto
    {
        public string? ToLocation { get; set; }
        public int? ProductionFacilityId { get; set; }
    }
}
