using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface ISalesOrdersService
    {
        Task<OrderEventDto> AddManualEvent(int orderId, OrderEventCreateDto dto);

        Task<SalesOrderDto> CreateSalesOrderAsync(SalesOrderCreateDto dto, string userId);

        Task<SalesOrderDto?> GetSalesOrderAsync(int id);

        Task<IList<SalesOrderDto>> GetSalesOrdersAsync();

        Task<OrderEventDto> UpdateEvent(int id, int orderId, OrderEventUpdateDto dto);

        Task<SalesOrderDto> UpdateSalesOrderAsync(int id, SalesOrderUpdateDto dto, string userId);
    }
}
