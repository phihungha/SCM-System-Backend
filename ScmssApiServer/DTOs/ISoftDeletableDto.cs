namespace ScmssApiServer.DTOs
{
    public interface ISoftDeletableDto
    {
        DateTime CreateTime { get; set; }
        bool IsActive { get; set; }
        DateTime? UpdateTime { get; set; }
    }
}
