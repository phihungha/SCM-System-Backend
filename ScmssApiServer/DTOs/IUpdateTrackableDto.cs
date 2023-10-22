namespace ScmssApiServer.DTOs
{
    public interface IUpdateTrackableDto
    {
        bool IsActive { get; set; }
        DateTime CreatedTime { get; set; }
        DateTime? UpdatedTime { get; set; }
    }
}
