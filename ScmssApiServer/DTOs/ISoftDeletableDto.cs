namespace ScmssApiServer.DTOs
{
    public interface ISoftDeletableDto : ICreateUpdateTimeDto
    {
        bool IsActive { get; set; }
    }
}
