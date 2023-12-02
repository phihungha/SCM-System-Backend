using ScmssApiServer.DTOs;
using System.Security.Claims;

namespace ScmssApiServer.IDomainServices
{
    public interface IProductsService
    {

        Task<IList<ProductDto>> GetManyAsync();
        Task<ProductDto?> GetAsync(int id);
        Task<ProductDto> CreateAsync(ProductInputDto dto);
        Task<ProductDto> UpdateAsync(int id, ProductInputDto dto);
        Task DeleteAsync(int id);
    }
}
