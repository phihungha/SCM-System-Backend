using ScmssApiServer.DTOs;
using ScmssApiServer.Models;

namespace ScmssApiServer.IDomainServices
{
    public interface IConfigService
    {
        public Task<Config> GetAsync();

        public Task<Config> SetAsync(ConfigInputDto dto);
    }
}
