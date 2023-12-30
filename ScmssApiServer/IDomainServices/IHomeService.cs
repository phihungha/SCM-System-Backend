using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IHomeService
    {
        Task<HomeDto> GetHome();
    }
}
