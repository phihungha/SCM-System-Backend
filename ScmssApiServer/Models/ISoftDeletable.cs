namespace ScmssApiServer.Models
{
    /// <summary>
    /// Interface for models that have soft-delete ability.
    /// </summary>
    public interface ISoftDeletable : ICreateUpdateTime
    {
        bool IsActive { get; set; }
    }
}
