using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class SuppliesService : ISuppliesService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public SuppliesService(IMapper mapper, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<SupplyDto> AddAsync(SupplyInputDto dto)
        {
            var supply = _mapper.Map<Supply>(dto);
            _dbContext.Add(supply);
            await _dbContext.SaveChangesAsync();

            IList<ProductionFacility> facilities = await _dbContext.ProductionFacilities.ToListAsync();
            foreach (ProductionFacility facility in facilities)
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
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(supply).Reference(i => i.Vendor).LoadAsync();
            return _mapper.Map<SupplyDto>(supply);
        }

        public async Task<SupplyDto?> GetAsync(int id)
        {
            Supply? supply = await _dbContext.Supplies
                .AsNoTracking()
                .Include(i => i.Vendor)
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<SupplyDto?>(supply);
        }

        public async Task<IList<SupplyDto>> GetManyAsync(SimpleQueryDto dto)
        {
            string? searchTerm = dto.SearchTerm?.ToLower();
            SimpleSearchCriteria? searchCriteria = dto.SearchCriteria;
            bool? displayAll = dto.All;

            var query = _dbContext.Supplies.AsNoTracking();

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

            IList<Supply> supplies = await query.Include(i => i.Vendor)
                                                .OrderBy(i => i.Id)
                                                .ToListAsync();
            return _mapper.Map<IList<SupplyDto>>(supplies);
        }

        public async Task<SupplyDto> UpdateAsync(int id, SupplyInputDto dto)
        {
            Supply? supply = await _dbContext.Supplies
                .Include(i => i.Vendor)
                .Include(i => i.WarehouseSupplyItems)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (supply == null)
            {
                throw new EntityNotFoundException();
            }

            _mapper.Map(dto, supply);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<SupplyDto>(supply);
        }
    }
}
