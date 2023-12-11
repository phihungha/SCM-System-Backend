using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IProductionFacilitiesService
    {
        public Task<ProductionFacilityDto> Add(ProductionFacilityInputDto dto);

        public Task<ProductionFacilityDto> Delete(int id);

        public Task<ProductionFacilityDto> Get(int id);

        public Task<IList<ProductionFacilityDto>> GetMany(SimpleQueryDto queryDto);

        public Task<ProductionFacilityDto> Update(int id, ProductionFacilityInputDto dto);
    }
}
