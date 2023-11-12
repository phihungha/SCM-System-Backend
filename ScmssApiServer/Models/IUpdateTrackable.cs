namespace ScmssApiServer.Models
{
    public interface IUpdateTrackable
    {
        bool IsActive { get; set; }
        DateTime CreateTime { get; set; }
        DateTime? UpdateTime { get; set; }
    }
}
