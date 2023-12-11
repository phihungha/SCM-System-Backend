using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;

namespace ScmssApiServer.DomainServices
{
    public class ProductionFacilitiesService : IProductionFacilitiesService
    {
        public Task<ProductionFacilityDto> Add(ProductionFacilityInputDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ProductionFacilityDto> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductionFacilityDto> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ProductionFacilityDto>> GetMany(SimpleQueryDto queryDto)
        {
            throw new NotImplementedException();
        }

        public Task<ProductionFacilityDto> Update(int id, ProductionFacilityInputDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
