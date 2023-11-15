using ScmssApiServer.DTOs;
using ScmssApiServer.Models;

namespace ScmssApiServer.IDomainServices
{
    public interface ISalesOrdersService
    {
        Task<IList<SalesOrderDto>> GetSalesOrdersAsync();

        Task<SalesOrderDto?> GetSalesOrderAsync(int id);

        Task<SalesOrderDto> CreateSalesOrderAsync(SalesOrderInputDto dto, string userId);

        Task<SalesOrderDto> UpdateSalesOrderAsync(int id, SalesOrderInputDto dto);

        Task<SalesOrderProgressUpdate> CreateProgressUpdateAsync(int id, OrderProgressUpdateInputDto dto);

        Task<SalesOrderProgressUpdate> EditProgressUpdateAsync(int id, OrderProgressUpdateInputDto dto);
    }
}
