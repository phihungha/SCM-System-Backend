using ScmssApiServer.DTOs;
using ScmssApiServer.Services;

namespace ScmssApiServer.IDomainServices
{
    public interface IProductionOrdersService
    {
        Task<ProductionOrderEventDto> AddManualEventAsync(int orderId, ProductionOrderEventCreateDto dto);

        Task<ProductionOrderDto> CreateAsync(ProductionOrderCreateDto dto, Identity identity);

        Task<ProductionOrderDto?> GetAsync(int id, Identity identity);

        Task<IList<ProductionOrderDto>> GetManyAsync(ProductionOrderQueryDto dto, Identity identity);

        Task<ProductionOrderDto> UpdateAsync(int id, ProductionOrderUpdateDto dto, Identity identity);

        Task<ProductionOrderEventDto> UpdateEventAsync(int id, int orderId, OrderEventUpdateDto dto);
    }
}
