namespace ScmssApiServer.DTOs
{
    public class SalesOrderDto : TransOrderDto
    {
        public CompanyDto Customer { get; set; } = null!;
        public int CustomerId { get; set; }
        public string? FromLocation { get; set; }

        public ICollection<SalesOrderItemDto> Items { get; set; }
            = new List<SalesOrderItemDto>();

        public ProductionFacilityDto? ProductionFacility { get; set; }
        public int? ProductionFacilityId { get; set; }
        public string? ToLocation { get; set; }
    }
}
