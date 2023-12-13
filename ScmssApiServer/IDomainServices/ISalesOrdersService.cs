using ScmssApiServer.DTOs;
using ScmssApiServer.Services;

namespace ScmssApiServer.IDomainServices
{
    public interface ISalesOrdersService
    {
        Task<TransOrderEventDto> AddManualEventAsync(int orderId, TransOrderEventCreateDto dto);

        Task<SalesOrderDto> CreateAsync(SalesOrderCreateDto dto, Identity identity);

        Task<SalesOrderDto?> GetAsync(int id, Identity identity);

        Task<IList<SalesOrderDto>> GetManyAsync(Identity identity);

        Task<SalesOrderDto> UpdateAsync(int id, SalesOrderUpdateDto dto, Identity identity);

        Task<TransOrderEventDto> UpdateEventAsync(int id, int orderId, OrderEventUpdateDto dto);
    }
}
