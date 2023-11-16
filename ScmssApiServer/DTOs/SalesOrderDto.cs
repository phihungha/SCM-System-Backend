namespace ScmssApiServer.DTOs
{
    public class SalesOrderDto : OrderDto
    {
        public ICollection<SalesOrderItemDto> Items { get; set; }
            = new List<SalesOrderItemDto>();

        public string? FromLocation { get; set; }
        public string? ToLocation { get; set; }

        public int? ProductionFacilityId { get; set; }
        public ProductionFacilityDto? ProductionFacility { get; set; }
        public int CustomerId { get; set; }
        public CompanyDto Customer { get; set; } = null!;
    }
}
