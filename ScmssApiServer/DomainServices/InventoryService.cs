using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;
using ScmssApiServer.Services;

namespace ScmssApiServer.DomainServices
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public InventoryService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<WarehouseProductItemDto?> GetProduct(int facilityId, int id, Identity identity)
        {
            WarehouseProductItem? item = await _dbContext.WarehouseProductItems
                .AsNoTracking()
                .Include(i => i.Product)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Events)
                .Where(i => i.ProductionFacilityId == facilityId)
                .FirstOrDefaultAsync(i => i.ProductId == id);
            return _mapper.Map<WarehouseProductItemDto?>(item);
        }

        public Task<IList<ProductionOrderDto>> GetProductionOrdersForIssue(int facilityId, InventoryOrderQueryDto query, Identity identity)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ProductionOrderDto>> GetProductionOrdersForReceive(int facilityId, InventoryOrderQueryDto query, Identity identity)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<WarehouseProductItemDto>?> GetProducts(
            int facilityId, SimpleQueryDto dto, Identity identity)
        {
            string? searchTerm = dto.SearchTerm?.ToLower();
            SimpleSearchCriteria? searchCriteria = dto.SearchCriteria;
            bool? displayAll = dto.All;

            var query = _dbContext.WarehouseProductItems
                .AsNoTracking()
                .Include(i => i.Product)
                .Where(i => i.ProductionFacilityId == facilityId);

            if (searchTerm != null)
            {
                if (searchCriteria == SimpleSearchCriteria.Name)
                {
                    query = query.Where(i => i.Product.Name.ToLower().Contains(searchTerm));
                }
                else
                {
                    query = query.Where(i => i.ProductId == int.Parse(searchTerm));
                }
            }

            if (!displayAll ?? true)
            {
                query = query.Where(i => i.IsActive);
            }

            IList<WarehouseProductItem> items = await query.OrderBy(i => i.ProductId).ToListAsync();
            return _mapper.Map<IList<WarehouseProductItemDto>>(items);
        }

        public Task<IList<PurchaseOrderDto>> GetPurchaseOrdersForReceive(int facilityId, InventoryOrderQueryDto query, Identity identity)
        {
            throw new NotImplementedException();
        }

        public Task<IList<SalesOrderDto>> GetSalesOrdersForIssue(int facilityId, InventoryOrderQueryDto query, Identity identity)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<WarehouseSupplyItemDto>> GetSupplies(int facilityId, SimpleQueryDto dto, Identity identity)
        {
            string? searchTerm = dto.SearchTerm?.ToLower();
            SimpleSearchCriteria? searchCriteria = dto.SearchCriteria;
            bool? displayAll = dto.All;

            var query = _dbContext.WarehouseSupplyItems
                .AsNoTracking()
                .Include(i => i.Supply)
                .Where(i => i.ProductionFacilityId == facilityId);

            if (searchTerm != null)
            {
                if (searchCriteria == SimpleSearchCriteria.Name)
                {
                    query = query.Where(i => i.Supply.Name.ToLower().Contains(searchTerm));
                }
                else
                {
                    query = query.Where(i => i.SupplyId == int.Parse(searchTerm));
                }
            }

            if (!displayAll ?? true)
            {
                query = query.Where(i => i.IsActive);
            }

            IList<WarehouseSupplyItem> items = await query.OrderBy(i => i.SupplyId).ToListAsync();
            return _mapper.Map<IList<WarehouseSupplyItemDto>>(items);
        }

        public async Task<WarehouseSupplyItemDto?> GetSupply(int facilityId, int id, Identity identity)
        {
            WarehouseSupplyItem? item = await _dbContext.WarehouseSupplyItems
                .AsNoTracking()
                .Include(i => i.Supply)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Events)
                .Where(i => i.ProductionFacilityId == facilityId)
                .FirstOrDefaultAsync(i => i.SupplyId == id);
            return _mapper.Map<WarehouseSupplyItemDto?>(item);
        }

        public Task<IList<WarehouseProductItemDto>> UpdateProducts(int facilityId, WarehouseUpdateDto<WarehouseProductItemInputDto> dto, Identity identity)
        {
            throw new NotImplementedException();
        }

        public Task<IList<WarehouseSupplyItemDto>> UpdateSupplies(int facilityId, WarehouseUpdateDto<WarehouseSupplyItemInputDto> dto, Identity identity)
        {
            throw new NotImplementedException();
        }
    }
}
