using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IProductsService
    {
        Task<ProductDto> AddAsync(ProductInputDto dto);

        string GenerateImageUploadUrl(int id);

        Task<ProductDto?> GetAsync(int id);

        Task<IList<ProductDto>> GetManyAsync(SimpleQueryDto queryDto);

        Task<ProductDto> UpdateAsync(int id, ProductInputDto dto);
    }
}
