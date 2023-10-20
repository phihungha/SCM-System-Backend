namespace ScmssApiServer.Models
{
    public interface IUpdateTrackable
    {
        bool IsActive { get; set; }
        DateTime CreatedTime { get; set; }
        DateTime? UpdatedTime { get; set; }
    }
}
