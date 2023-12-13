using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface ISalesOrdersService
    {
        Task<TransOrderEventDto> AddManualEventAsync(int orderId, TransOrderEventCreateDto dto);

        Task<SalesOrderDto> CreateAsync(SalesOrderCreateDto dto, string userId);

        Task<SalesOrderDto?> GetAsync(int id, string userId);

        Task<IList<SalesOrderDto>> GetManyAsync(string userId);

        Task<SalesOrderDto> UpdateAsync(int id, SalesOrderUpdateDto dto, string userId);

        Task<TransOrderEventDto> UpdateEventAsync(int id, int orderId, OrderEventUpdateDto dto);
    }
}
