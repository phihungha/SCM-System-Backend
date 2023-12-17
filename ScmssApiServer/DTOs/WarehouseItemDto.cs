namespace ScmssApiServer.DTOs
{
    public abstract class WarehouseItemDto<T> : ICreateUpdateTimeDto
        where T : WarehouseItemEventDto
    {
        public decimal TotalValue;
        public DateTime CreateTime { get; set; }
        public ICollection<T> Events { get; set; } = new List<T>();
        public required ProductionFacilityDto ProductionFacility { get; set; }
        public int ProductionFacilityId { get; set; }
        public double Quantity { get; set; }
        public required string Unit { get; set; }
        public decimal UnitValue { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
