namespace ScmssApiServer.Models
{
    public class Company
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Location { get; set; }

        public string? Description { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ContactPerson { get; set; }
    }
}
