using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;

namespace ScmssApiServer.DomainServices
{
    public class ProductionFacilitiesService : IProductionFacilitiesService
    {
        public Task<ProductionFacilityDto> AddAsync(ProductionFacilityInputDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ProductionFacilityDto> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ProductionFacilityDto>> GetManyAsync(SimpleQueryDto queryDto)
        {
            throw new NotImplementedException();
        }

        public Task<ProductionFacilityDto> UpdateAsync(int id, ProductionFacilityInputDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
