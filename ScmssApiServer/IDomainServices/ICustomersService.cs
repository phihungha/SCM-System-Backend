using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface ICustomersService
    {
        public Task<CompanyDto> Add(CompanyInputDto dto);

        public Task<CompanyDto> Delete(int id);

        public Task<CompanyDto> Get(int id);

        public Task<IList<CompanyDto>> GetMany(SimpleQueryDto queryDto);

        public Task<CompanyDto> Update(int id, CompanyInputDto dto);
    }
}
