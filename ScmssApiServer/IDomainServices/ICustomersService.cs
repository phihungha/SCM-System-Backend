using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface ICustomersService
    {
        public Task<CompanyDto> AddAsync(CompanyInputDto dto);

        public Task<CompanyDto> GetAsync(int id);

        public Task<IList<CompanyDto>> GetManyAsync(SimpleQueryDto queryDto);

        public Task<CompanyDto> UpdateAsync(int id, CompanyInputDto dto);
    }
}
