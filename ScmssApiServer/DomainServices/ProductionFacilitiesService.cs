using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class ProductionFacilitiesService : IProductionFacilitiesService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductionFacilitiesService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProductionFacilityDto> AddAsync(ProductionFacilityInputDto dto)
        {
            var facility = _mapper.Map<ProductionFacility>(dto);
            _dbContext.Add(facility);
            await _dbContext.SaveChangesAsync();

            IList<Supply> supplies = await _dbContext.Supplies.ToListAsync();
            foreach (Supply supply in supplies)
            {
                var warehouseItem = new WarehouseSupplyItem
                {
                    SupplyId = supply.Id,
                    Supply = supply,
                    ProductionFacilityId = facility.Id,
                    ProductionFacility = facility,
                    Quantity = 0,
                };

                warehouseItem.Events.Add(new WarehouseSupplyItemEvent
                {
                    Time = DateTime.UtcNow,
                    Quantity = 0,
                    Change = 0,
                    WarehouseSupplyItem = warehouseItem,
                    WarehouseSupplyItemSupplyId = warehouseItem.SupplyId,
                    WarehouseSupplyItemProductionFacilityId = warehouseItem.ProductionFacilityId,
                });

                facility.WarehouseSupplyItems.Add(warehouseItem);
            }

            IList<Product> products = await _dbContext.Products.ToListAsync();
            foreach (Product product in products)
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

            return _mapper.Map<ProductionFacilityDto>(facility);
        }

        public async Task<ProductionFacilityDto?> GetAsync(int id)
        {
            ProductionFacility? facility = await _dbContext.ProductionFacilities
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<ProductionFacilityDto?>(facility);
        }

        public async Task<IList<ProductionFacilityDto>> GetManyAsync(SimpleQueryDto dto)
        {
            string? searchTerm = dto.SearchTerm?.ToLower();
            SimpleSearchCriteria? searchCriteria = dto.SearchCriteria;
            bool? displayAll = dto.All;

            var query = _dbContext.ProductionFacilities.AsNoTracking();

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

            IList<ProductionFacility> facilities = await query.OrderBy(i => i.Id).ToListAsync();
            return _mapper.Map<IList<ProductionFacilityDto>>(facilities);
        }

        public async Task<ProductionFacilityDto> UpdateAsync(int id, ProductionFacilityInputDto dto)
        {
            ProductionFacility? facility = await _dbContext.ProductionFacilities.FindAsync(id);
            if (facility == null)
            {
                throw new EntityNotFoundException();
            }

            if (!dto.IsActive)
            {
                IList<string> activeUsers = await _dbContext.Users
                    .Where(i => i.IsActive)
                    .Where(i => i.ProductionFacilityId == id)
                    .Select(i => i.Name)
                    .ToListAsync();
                if (activeUsers.Count > 0)
                {
                    throw new InvalidDomainOperationException(
                            $"There are still active users in this facility: {string.Join(',', activeUsers)}"
                        );
                }
            }

            _mapper.Map(dto, facility);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProductionFacilityDto>(facility);
        }
    }
}
