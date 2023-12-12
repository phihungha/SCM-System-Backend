using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IProductionOrdersService
    {
        Task<ProductionOrderEventDto> AddManualEventAsync(int orderId, ProductionOrderEventCreateDto dto);

        Task<ProductionOrderDto> CreateAsync(OrderCreateDto<OrderItemInputDto> dto, string userId);

        Task<ProductionOrderDto?> GetAsync(int id, string userId);

        Task<IList<ProductionOrderDto>> GetManyAsync(string userId);

        Task<ProductionOrderDto> UpdateAsync(int id, ProductionOrderUpdateDto dto, string userId);

        Task<ProductionOrderEventDto> UpdateEventAsync(int id, int orderId, OrderEventUpdateDto dto);
    }
}
