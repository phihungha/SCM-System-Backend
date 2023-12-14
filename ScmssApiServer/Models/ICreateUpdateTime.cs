namespace ScmssApiServer.Models
{
    public interface ICreateUpdateTime
    {
        DateTime CreateTime { get; set; }
        DateTime? UpdateTime { get; set; }
    }
}
