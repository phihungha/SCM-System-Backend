using ScmssApiServer.DTOs;
using ScmssApiServer.Models;

namespace ScmssApiServer.IDomainServices
{
    public interface ISalesOrdersService
    {
        Task<IList<SalesOrderDto>> GetSalesOrdersAsync();

        Task<SalesOrderDto?> GetSalesOrderAsync(int id);

        Task<SalesOrderDto> CreateSalesOrderAsync(SalesOrderCreateDto dto, string userId);

        Task<SalesOrderDto> UpdateSalesOrderAsync(int id, SalesOrderInputDto dto, string userId);

        Task<SalesOrderEvent> AddManualEvent(int orderId, OrderEventInputDto dto);

        Task<SalesOrderEvent> EditEvent(int id, int orderId, OrderEventInputDto dto);
    }
}
