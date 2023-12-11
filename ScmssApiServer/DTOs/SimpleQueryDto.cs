namespace ScmssApiServer.DTOs
{
    public class SimpleQueryDto
    {
        public string? SearchTerm { get; set; }
        public SimpleSearchCriteria? SearchCriteria { get; set; }
        public bool? All { get; set; }
    }
}
