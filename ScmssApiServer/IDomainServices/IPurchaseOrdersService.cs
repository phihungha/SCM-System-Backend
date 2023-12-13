using ScmssApiServer.DTOs;
using ScmssApiServer.Services;

namespace ScmssApiServer.IDomainServices
{
    public interface IPurchaseOrdersService
    {
        Task<TransOrderEventDto> AddManualEventAsync(int orderId, TransOrderEventCreateDto dto);

        Task<PurchaseOrderDto> CreateAsync(PurchaseOrderCreateDto dto, Identity identity);

        Task<PurchaseOrderDto?> GetAsync(int id, Identity identity);

        Task<IList<PurchaseOrderDto>> GetManyAsync(Identity identity);

        Task<PurchaseOrderDto> UpdateAsync(int id, PurchaseOrderUpdateDto dto, Identity identity);

        Task<TransOrderEventDto> UpdateEventAsync(int id, int orderId, OrderEventUpdateDto dto);
    }
}
