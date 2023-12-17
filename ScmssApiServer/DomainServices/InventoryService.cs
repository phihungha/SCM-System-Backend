using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DTOs;
using ScmssApiServer.Exceptions;
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
            IsAuthorizedOrThrow(identity, facilityId);

            WarehouseProductItem? item = await _dbContext.WarehouseProductItems
                .AsNoTracking()
                .Include(i => i.Product)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Events)
                .Where(i => i.ProductionFacilityId == facilityId)
                .FirstOrDefaultAsync(i => i.ProductId == id);
            return _mapper.Map<WarehouseProductItemDto?>(item);
        }

        public async Task<IList<ProductionOrderDto>> GetProductionOrdersToIssue(
            int facilityId, InventoryOrderQueryDto dto, Identity identity)
        {
            IsAuthorizedOrThrow(identity, facilityId);

            var query = _dbContext.ProductionOrders
                .AsNoTracking()
                .Where(i => i.ProductionFacilityId == facilityId)
                .Where(i => i.ApprovalStatus == ApprovalStatus.Approved);

            if (dto.Id != null)
            {
                query = query.Where(i => i.Id == dto.Id);
            }

            if (!dto.All ?? true)
            {
                query = query.Where(i => i.Status == OrderStatus.Processing);
            }

            IList<ProductionOrder> orders = await query
                .Include(i => i.ProductionFacility)
                .Include(i => i.CreateUser)
                .Include(i => i.ApproveProductionManager)
                .Include(i => i.EndUser)
                .OrderBy(i => i.Id)
                .ToListAsync();
            return _mapper.Map<IList<ProductionOrderDto>>(orders);
        }

        public async Task<IList<ProductionOrderDto>> GetProductionOrdersToReceive(
            int facilityId, InventoryOrderQueryDto dto, Identity identity)
        {
            IsAuthorizedOrThrow(identity, facilityId);

            var query = _dbContext.ProductionOrders
                .AsNoTracking()
                .Where(i => i.ProductionFacilityId == facilityId);

            if (dto.Id != null)
            {
                query = query.Where(i => i.Id == dto.Id);
            }

            if (!dto.All ?? true)
            {
                query = query.Where(i => i.Status == OrderStatus.WaitingAcceptance);
            }

            IList<ProductionOrder> orders = await query
                .Include(i => i.ProductionFacility)
                .Include(i => i.CreateUser)
                .Include(i => i.ApproveProductionManager)
                .Include(i => i.EndUser)
                .OrderBy(i => i.Id)
                .ToListAsync();
            return _mapper.Map<IList<ProductionOrderDto>>(orders);
        }

        public async Task<IList<WarehouseProductItemDto>> GetProducts(
            int facilityId, SimpleQueryDto dto, Identity identity)
        {
            IsAuthorizedOrThrow(identity, facilityId);

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
                query = query.Where(i => i.Product.IsActive);
            }

            IList<WarehouseProductItem> items = await query.OrderBy(i => i.ProductId).ToListAsync();
            return _mapper.Map<IList<WarehouseProductItemDto>>(items);
        }

        public async Task<IList<PurchaseOrderDto>> GetPurchaseOrdersToReceive(
            int facilityId, InventoryOrderQueryDto dto, Identity identity)
        {
            IsAuthorizedOrThrow(identity, facilityId);

            var query = _dbContext.PurchaseOrders
                .AsNoTracking()
                .Where(i => i.ProductionFacilityId == facilityId);

            if (dto.Id != null)
            {
                query = query.Where(i => i.Id == dto.Id);
            }

            if (!dto.All ?? true)
            {
                query = query.Where(i => i.Status == OrderStatus.WaitingAcceptance);
            }

            IList<PurchaseOrder> orders = await query
                .Include(i => i.ProductionFacility)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .OrderBy(i => i.Id)
                .ToListAsync();
            return _mapper.Map<IList<PurchaseOrderDto>>(orders);
        }

        public async Task<IList<SalesOrderDto>> GetSalesOrdersToIssue(
            int facilityId, InventoryOrderQueryDto dto, Identity identity)
        {
            IsAuthorizedOrThrow(identity, facilityId);

            var query = _dbContext.SalesOrders
                .AsNoTracking()
                .Where(i => i.ProductionFacilityId == facilityId);

            if (dto.Id != null)
            {
                query = query.Where(i => i.Id == dto.Id);
            }

            if (!dto.All ?? true)
            {
                query = query.Where(i => i.Status == OrderStatus.Processing);
            }

            IList<SalesOrder> orders = await query
                .Include(i => i.ProductionFacility)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .OrderBy(i => i.Id)
                .ToListAsync();
            return _mapper.Map<IList<SalesOrderDto>>(orders);
        }

        public async Task<IList<WarehouseSupplyItemDto>> GetSupplies(int facilityId, SimpleQueryDto dto, Identity identity)
        {
            IsAuthorizedOrThrow(identity, facilityId);

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
                query = query.Where(i => i.Supply.IsActive);
            }

            IList<WarehouseSupplyItem> items = await query.OrderBy(i => i.SupplyId).ToListAsync();
            return _mapper.Map<IList<WarehouseSupplyItemDto>>(items);
        }

        public async Task<WarehouseSupplyItemDto?> GetSupply(int facilityId, int id, Identity identity)
        {
            IsAuthorizedOrThrow(identity, facilityId);

            WarehouseSupplyItem? item = await _dbContext.WarehouseSupplyItems
                .AsNoTracking()
                .Include(i => i.Supply)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Events)
                .Where(i => i.ProductionFacilityId == facilityId)
                .FirstOrDefaultAsync(i => i.SupplyId == id);
            return _mapper.Map<WarehouseSupplyItemDto?>(item);
        }

        public async Task<IList<WarehouseProductItemDto>> UpdateProducts(
            int facilityId, WarehouseUpdateDto dto, Identity identity)
        {
            IsAuthorizedOrThrow(identity, facilityId);

            IList<int> itemIds = dto.Items.Select(i => i.Id).ToList();
            IDictionary<int, WarehouseProductItem> items = await _dbContext.WarehouseProductItems
                .Include(i => i.Product)
                .Include(i => i.Events)
                .Where(i => i.ProductionFacilityId == facilityId)
                .Where(i => itemIds.Contains(i.ProductId))
                .ToDictionaryAsync(i => i.ProductId);

            foreach (WarehouseItemInputDto input in dto.Items)
            {
                WarehouseProductItem item = items[input.Id];
                item.SetQuantityManually(input.Quantity);
            }

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<IList<WarehouseProductItemDto>>(items.Values);
        }

        public async Task<IList<WarehouseSupplyItemDto>> UpdateSupplies(
            int facilityId, WarehouseUpdateDto dto, Identity identity)
        {
            IsAuthorizedOrThrow(identity, facilityId);

            IList<int> itemIds = dto.Items.Select(i => i.Id).ToList();
            IDictionary<int, WarehouseSupplyItem> items = await _dbContext.WarehouseSupplyItems
                .Include(i => i.Supply)
                .Where(i => i.ProductionFacilityId == facilityId)
                .Where(i => itemIds.Contains(i.SupplyId))
                .ToDictionaryAsync(i => i.SupplyId);

            foreach (WarehouseItemInputDto input in dto.Items)
            {
                WarehouseSupplyItem item = items[input.Id];
                item.SetQuantityManually(input.Quantity);
            }

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<IList<WarehouseSupplyItemDto>>(items.Values);
        }

        private void IsAuthorizedOrThrow(Identity identity, int facilityId)
        {
            if (!identity.IsSuperUser && identity.ProductionFacilityId != facilityId)
            {
                throw new UnauthorizedException("Unauthorized to access inventory of another facility.");
            }
        }
    }
}
