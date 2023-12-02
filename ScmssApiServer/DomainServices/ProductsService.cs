using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class ProductsService : IProductsService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductsService(IMapper mapper, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateAsync(ProductInputDto dto)
        {
            var item = _mapper.Map<Product>(dto);
            _dbContext.Add(item);
            await _dbContext.SaveChangesAsync();
            item = await _dbContext.Products.Include(i => i.SupplyCostItems)
                                            .ThenInclude(i => i.Supply)
                                            .FirstAsync(x => x.Id == item.Id);
            return GetProductDto(item);
        }

        public async Task DeleteAsync(int id)
        {
            Product? item = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                throw new EntityNotFoundException();
            }
            item.IsActive = false;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ProductDto?> GetAsync(int id)
        {
            Product? users = await _dbContext.Products.Include(i => i.SupplyCostItems)
                                                      .ThenInclude(i => i.Supply)
                                                      .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ProductDto?>(users);
        }

        public async Task<IList<ProductDto>> GetManyAsync()
        {
            IList<Product> users = await _dbContext.Products.ToListAsync();
            return _mapper.Map<IList<ProductDto>>(users);
        }

        public async Task<ProductDto> UpdateAsync(int id, ProductInputDto dto)
        {
            Product? item = await _dbContext.Products.Include(i => i.SupplyCostItems)
                                                     .ThenInclude(i => i.Supply)
                                                     .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                throw new EntityNotFoundException();
            }

            _dbContext.RemoveRange(item.SupplyCostItems);
            _mapper.Map(dto, item);

            await _dbContext.SaveChangesAsync();
            return GetProductDto(item);
        }

        private ProductDto GetProductDto(Product product)
        {
            return _mapper.Map<ProductDto>(product);
        }
    }
}
