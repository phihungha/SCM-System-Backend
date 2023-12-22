using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.IServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class ProductsService : IProductsService
    {
        private readonly AppDbContext _dbContext;
        private readonly IFileHostService _fileHostService;
        private readonly IMapper _mapper;

        public ProductsService(IFileHostService fileHostService,
                               AppDbContext dbContext,
                               IMapper mapper)
        {
            _dbContext = dbContext;
            _fileHostService = fileHostService;
            _mapper = mapper;
        }

        public async Task<ProductDto> AddAsync(ProductInputDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            _dbContext.Add(product);
            await _dbContext.SaveChangesAsync();
            product = await _dbContext.Products.Include(i => i.SupplyCostItems)
                                               .ThenInclude(i => i.Supply)
                                               .SingleAsync(i => i.Id == product.Id);

            IList<ProductionFacility> facilities = await _dbContext.ProductionFacilities.ToListAsync();
            foreach (ProductionFacility facility in facilities)
            {
                var warehouseItem = new WarehouseProductItem
                {
                    ProductId = product.Id,
                    Product = product,
                    ProductionFacilityId = facility.Id,
                    ProductionFacility = facility,
                    Quantity = 0,
                };

                warehouseItem.Events.Add(new WarehouseProductItemEvent
                {
                    Time = DateTime.UtcNow,
                    Quantity = 0,
                    Change = 0,
                    WarehouseProductItem = warehouseItem,
                    WarehouseProductItemProductId = warehouseItem.ProductId,
                    WarehouseProductItemProductionFacilityId = warehouseItem.ProductionFacilityId,
                });

                facility.WarehouseProductItems.Add(warehouseItem);
            }
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public string GenerateImageUploadUrl(int id)
        {
            return _fileHostService.GenerateUploadUrl(Product.ImageFolderKey, id);
        }

        public async Task<ProductDto?> GetAsync(int id)
        {
            Product? product = await _dbContext.Products
                .AsNoTracking()
                .Include(i => i.SupplyCostItems)
                .ThenInclude(i => i.Supply)
                .SingleOrDefaultAsync(i => i.Id == id);
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

            IList<Product> products = await query.OrderBy(i => i.Id).ToListAsync();
            return _mapper.Map<IList<ProductDto>>(products);
        }

        public async Task<ProductDto> UpdateAsync(int id, ProductInputDto dto)
        {
            Product? product = await _dbContext.Products
                .Include(i => i.WarehouseProductItems)
                .Include(i => i.SupplyCostItems)
                .ThenInclude(i => i.Supply)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                throw new EntityNotFoundException();
            }

            _dbContext.RemoveRange(product.SupplyCostItems);
            _mapper.Map(dto, product);

            await _dbContext.SaveChangesAsync();
            product = await _dbContext.Products.Include(i => i.SupplyCostItems)
                                               .ThenInclude(i => i.Supply)
                                               .SingleAsync(i => i.Id == product.Id);

            return _mapper.Map<ProductDto>(product);
        }
    }
}
