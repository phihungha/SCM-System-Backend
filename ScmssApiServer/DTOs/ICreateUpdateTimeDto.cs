namespace ScmssApiServer.DTOs
{
    public interface ICreateUpdateTimeDto
    {
        DateTime CreateTime { get; set; }
        DateTime? UpdateTime { get; set; }
    }
}
