namespace ScmssApiServer.DTOs
{
    public interface IUpdateTrackableDto
    {
        bool IsActive { get; set; }
        DateTime CreateTime { get; set; }
        DateTime? UpdateTime { get; set; }
    }
}
