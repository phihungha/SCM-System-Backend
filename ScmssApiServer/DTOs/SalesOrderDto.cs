namespace ScmssApiServer.DTOs
{
    public class SalesOrderDto : OrderDto
    {
        public required string ToLocation { get; set; }

        public int? ProductionFacilityId { get; set; }
        public ProductionFacilityDto? ProductionFacility { get; set; }
        public int CustomerId { get; set; }
        public CompanyDto Customer { get; set; } = null!;
    }
}
