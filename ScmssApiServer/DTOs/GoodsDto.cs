namespace ScmssApiServer.DTOs
{
    public class GoodsDto : ISoftDeletableDto
    {
        public DateTime CreateTime { get; set; }
        public string? Description { get; set; }
        public int ExpirationMonth { get; set; }
        public int Id { get; set; }
        public string? ImageName { get; set; }
        public Uri? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public required string Unit { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
