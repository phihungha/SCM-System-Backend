using ScmssApiServer.DTOs;
using ScmssApiServer.Models;

namespace ScmssApiServer.IDomainServices
{
    public interface ISalesOrdersService
    {
        Task<IList<SalesOrderDto>> GetSalesOrdersAsync();

        Task<SalesOrderDto?> GetSalesOrderAsync(int id);

        Task<SalesOrderDto> CreateSalesOrderAsync(SalesOrderCreateDto dto, string userId);

        Task<SalesOrderDto> UpdateSalesOrderAsync(int id, SalesOrderUpdateDto dto, string userId);

        Task<OrderEventDto> AddManualEvent(int orderId, OrderEventCreateDto dto);

        Task<OrderEventDto> UpdateEvent(int id, int orderId, OrderEventUpdateDto dto);
    }
}
