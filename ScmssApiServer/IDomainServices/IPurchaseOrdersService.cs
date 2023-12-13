using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IPurchaseOrdersService
    {
        Task<TransOrderEventDto> AddManualEventAsync(int orderId, TransOrderEventCreateDto dto);

        Task<PurchaseOrderDto> CreateAsync(PurchaseOrderCreateDto dto, string userId);

        Task<PurchaseOrderDto?> GetAsync(int id, string userId);

        Task<IList<PurchaseOrderDto>> GetManyAsync(string userId);

        Task<PurchaseOrderDto> UpdateAsync(int id, PurchaseOrderUpdateDto dto, string userId);

        Task<TransOrderEventDto> UpdateEventAsync(int id, int orderId, OrderEventUpdateDto dto);
    }
}
