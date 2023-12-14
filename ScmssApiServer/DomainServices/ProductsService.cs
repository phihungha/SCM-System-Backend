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
            var product = _mapper.Map<Product>(dto);
            _dbContext.Add(product);
            await _dbContext.SaveChangesAsync();
            product = await _dbContext.Products.Include(i => i.SupplyCostItems)
                                               .ThenInclude(i => i.Supply)
                                               .FirstAsync(x => x.Id == product.Id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto?> GetAsync(int id)
        {
            Product? product = await _dbContext.Products
                .AsNoTracking()
                .Include(i => i.SupplyCostItems)
                .ThenInclude(i => i.Supply)
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<ProductDto?>(product);
        }

        public async Task<IList<ProductDto>> GetManyAsync(SimpleQueryDto dto)
        {
            string? searchTerm = dto.SearchTerm?.ToLower();
            SimpleSearchCriteria? searchCriteria = dto.SearchCriteria;
            bool? displayAll = dto.All;

            var query = _dbContext.Products.AsNoTracking();

            if (searchTerm != null)
            {
                if (searchCriteria == SimpleSearchCriteria.Name)
                {
                    query = query.Where(i => i.Name.ToLower().Contains(searchTerm));
                }
                else
                {
                    query = query.Where(i => i.Id == int.Parse(searchTerm));
                }
            }

            if (!displayAll ?? true)
            {
                query = query.Where(i => i.IsActive);
            }

            IList<Product> products = await query.ToListAsync();
            return _mapper.Map<IList<ProductDto>>(products);
        }

        public async Task<ProductDto> UpdateAsync(int id, ProductInputDto dto)
        {
            Product? product = await _dbContext.Products.Include(i => i.SupplyCostItems)
                                                        .ThenInclude(i => i.Supply)
                                                        .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                throw new EntityNotFoundException();
            }

            _dbContext.RemoveRange(product.SupplyCostItems);
            _mapper.Map(dto, product);

            await _dbContext.SaveChangesAsync();
            product = await _dbContext.Products.Include(i => i.SupplyCostItems)
                                               .ThenInclude(i => i.Supply)
                                               .FirstAsync(x => x.Id == product.Id);
            return _mapper.Map<ProductDto>(product);
        }
    }
}
