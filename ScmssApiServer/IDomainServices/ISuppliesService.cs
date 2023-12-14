using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface ISuppliesService
    {
        Task<SupplyDto> AddAsync(SupplyInputDto dto);

        Task<SupplyDto?> GetAsync(int id);

        Task<IList<SupplyDto>> GetManyAsync(SimpleQueryDto queryDto);

        Task<SupplyDto> UpdateAsync(int id, SupplyInputDto dto);
    }
}
