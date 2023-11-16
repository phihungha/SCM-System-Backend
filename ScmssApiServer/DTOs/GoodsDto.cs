namespace ScmssApiServer.DTOs
{
    public abstract class GoodsDto : IUpdateTrackableDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Unit { get; set; }
        public decimal Price { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
