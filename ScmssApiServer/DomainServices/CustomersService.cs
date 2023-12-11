using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;

namespace ScmssApiServer.DomainServices
{
    public class CustomersService : ICustomersService
    {
        public Task<CompanyDto> Add(CompanyInputDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<CompanyDto> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CompanyDto> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<CompanyDto>> GetMany(SimpleQueryDto queryDto)
        {
            throw new NotImplementedException();
        }

        public Task<CompanyDto> Update(int id, CompanyInputDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
