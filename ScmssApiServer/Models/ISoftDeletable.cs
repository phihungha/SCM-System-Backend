namespace ScmssApiServer.Models
{
    /// <summary>
    /// Interface for models that have soft-delete ability.
    /// </summary>
    public interface ISoftDeletable
    {
        DateTime CreateTime { get; set; }
        bool IsActive { get; set; }
        DateTime? UpdateTime { get; set; }
    }
}
