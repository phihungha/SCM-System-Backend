namespace ScmssApiServer.Models
{
    public abstract class Goods : IUpdateTrackable
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public required string Unit { get; set; }

        public decimal Price { get; set; }

        public ICollection<ProductionFacility> ProductionFacilities { get; set; }
            = new List<ProductionFacility>();

        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? FinishTime { get; set; }
    }
}
