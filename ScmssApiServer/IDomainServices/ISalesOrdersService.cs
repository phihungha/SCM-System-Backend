using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface ISalesOrdersService
    {
        Task<TransOrderEventDto> AddManualEvent(int orderId, TransOrderEventCreateDto dto);

        Task<SalesOrderDto> CreateSalesOrderAsync(SalesOrderCreateDto dto, string userId);

        Task<SalesOrderDto?> GetSalesOrderAsync(int id);

        Task<IList<SalesOrderDto>> GetSalesOrdersAsync();

        Task<TransOrderEventDto> UpdateEvent(int id, int orderId, OrderEventUpdateDto dto);

        Task<SalesOrderDto> UpdateSalesOrderAsync(int id, SalesOrderUpdateDto dto, string userId);
    }
}
