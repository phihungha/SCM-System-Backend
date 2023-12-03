using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;

namespace ScmssApiServer.DomainServices
{
    public class PurchaseOrdersService : IPurchaseOrdersService
    {
        public Task<TransOrderEventDto> AddManualEventAsync(int orderId, TransOrderEventCreateDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseOrderDto> CreateAsync(PurchaseOrderCreateDto dto, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseOrderDto?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<PurchaseOrderDto>> GetManyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseOrderDto> UpdateAsync(int id, PurchaseOrderUpdateDto dto, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<TransOrderEventDto> UpdateEventAsync(int id, int orderId, OrderEventUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
