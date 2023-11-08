namespace ScmssApiServer.Models
{
    public abstract class Goods
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public required string Unit { get; set; }

        public decimal Price { get; set; }
    }
}
