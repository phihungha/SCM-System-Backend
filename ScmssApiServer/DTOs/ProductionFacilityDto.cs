namespace ScmssApiServer.DTOs
{
    public class ProductionFacilityDto : ISoftDeletableDto
    {
        public DateTime CreateTime { get; set; }
        public required string Description { get; set; }
        public required string Email { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public required string Location { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
