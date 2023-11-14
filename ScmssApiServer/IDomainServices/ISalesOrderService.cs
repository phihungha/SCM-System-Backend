using ScmssApiServer.DTOs;
using ScmssApiServer.Models;

namespace ScmssApiServer.IDomainServices
{
    public interface ISalesOrderService
    {
        Task<IList<SalesOrder>> GetSalesOrdersAsync();

        Task<SalesOrder?> GetSalesOrderAsync(int id);

        Task<SalesOrder> CreateSalesOrderAsync(SalesOrderInputDto dto, string userId);

        Task<SalesOrder> UpdateSalesOrderAsync(int id, SalesOrderInputDto dto);

        Task<SalesOrder> CancelSalesOrderAsync(int id, string userId);

        Task<SalesOrder> CompleteSalesOrderAsync(int id, string userId);

        Task<SalesOrderProgressUpdate> CreateProgressUpdateAsync(int id, OrderProgressUpdateInputDto dto);

        Task<SalesOrderProgressUpdate> EditProgressUpdateAsync(int id, OrderProgressUpdateInputDto dto);
    }
}
