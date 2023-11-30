using ScmssApiServer.DTOs;
using ScmssApiServer.Models;

namespace ScmssApiServer.IDomainServices
{
    public interface IProductionOrdersService
    {
        Task<ProductionOrderEventDto> AddManualEventAsync(int orderId, ProductionOrderEventCreateDto dto);

        Task<ProductionOrderDto> CreateAsync(ProductionOrderCreateDto dto, string userId);

        Task<ProductionOrderDto?> GetAsync(int id);

        Task<IList<ProductionOrderDto>> GetManyAsync();

        Task<ProductionOrderEventDto> UpdateEventAsync(int id, int orderId, OrderEventUpdateDto dto);

        Task<ProductionOrderDto> UpdateAsync(int id, ProductionOrderUpdateDto dto, string userId);
    }
}
