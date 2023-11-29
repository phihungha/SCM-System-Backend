namespace ScmssApiServer.Models
{
    public interface ISoftDeletable
    {
        bool IsActive { get; set; }
        DateTime CreateTime { get; set; }
        DateTime? UpdateTime { get; set; }
    }
}
