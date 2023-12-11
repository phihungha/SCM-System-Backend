using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IProductionFacilitiesService
    {
        public Task<ProductionFacilityDto> AddAsync(ProductionFacilityInputDto dto);

        public Task<ProductionFacilityDto> GetAsync(int id);

        public Task<IList<ProductionFacilityDto>> GetManyAsync(SimpleQueryDto queryDto);

        public Task<ProductionFacilityDto> UpdateAsync(int id, ProductionFacilityInputDto dto);
    }
}
