using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;

namespace ScmssApiServer.DomainServices
{
    public class CustomersService : ICustomersService
    {
        public Task<CompanyDto> AddAsync(CompanyInputDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<CompanyDto> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<CompanyDto>> GetManyAsync(SimpleQueryDto queryDto)
        {
            throw new NotImplementedException();
        }

        public Task<CompanyDto> UpdateAsync(int id, CompanyInputDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
