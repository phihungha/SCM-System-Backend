using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class SalesOrdersService : ISalesOrdersService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public SalesOrdersService(IMapper mapper, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<SalesOrderEvent> AddManualEvent(int orderId, OrderEventInputDto dto)
        {
            var item = _mapper.Map<SalesOrderEvent>(dto);
            SalesOrder order = await GetSalesOrderOrThrowAsync(orderId);
            order.AddManualEvent(dto.Type, dto.Location, dto.Message);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<SalesOrderDto> CreateSalesOrderAsync(SalesOrderInputDto dto, string userId)
        {
            var item = _mapper.Map<SalesOrder>(dto);
            await AddOrderItemsFromDtos(item, dto.Items);
            item.Start(userId);
            _dbContext.SalesOrders.Add(item);
            await _dbContext.SaveChangesAsync();
            return GetSalesOrderDto(item);
        }

        public async Task<SalesOrderEvent> EditEvent(int id, int orderId, OrderEventInputDto dto)
        {
            var item = _mapper.Map<SalesOrderEvent>(dto);
            SalesOrder order = await GetSalesOrderOrThrowAsync(id);
            order.EditEvent(id, dto.Location, dto.Message);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<SalesOrderDto?> GetSalesOrderAsync(int id)
        {
            SalesOrder? item = await _dbContext.SalesOrders
                .Include(i => i.CreateUser)
                .Include(i => i.FinishUser)
                .Include(i => i.Events)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<SalesOrderDto?>(item);
        }

        public async Task<IList<SalesOrderDto>> GetSalesOrdersAsync()
        {
            IList<SalesOrder> items = await _dbContext.SalesOrders
                .Include(i => i.CreateUser)
                .Include(i => i.FinishUser)
                .Include(i => i.Events)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Items)
                .ToListAsync();
            return _mapper.Map<IList<SalesOrderDto>>(items);
        }

        public async Task<SalesOrderDto> UpdateSalesOrderAsync(int id, SalesOrderInputDto dto)
        {
            SalesOrder? item = await _dbContext.SalesOrders
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null)
            {
                throw new EntityNotFoundException();
            }

            _mapper.Map(dto, item);

            _dbContext.RemoveRange(item.Items);
            await AddOrderItemsFromDtos(item, dto.Items);

            await _dbContext.SaveChangesAsync();
            return GetSalesOrderDto(item);
        }

        private SalesOrderDto GetSalesOrderDto(SalesOrder item)
        {
            return _mapper.Map<SalesOrderDto>(item);
        }

        private async Task<SalesOrder> GetSalesOrderOrThrowAsync(int id)
        {
            SalesOrder? item = await _dbContext.SalesOrders.FindAsync(id);
            if (item == null)
            {
                throw new EntityNotFoundException();
            }
            return item;
        }

        private async Task AddOrderItemsFromDtos(SalesOrder order, IEnumerable<OrderItemInputDto> dtos)
        {
            IList<int> productIds = dtos.Select(i => i.ItemId).ToList();
            IDictionary<int, Product> products = await _dbContext
                .Products
                .Where(i => productIds.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id);

            order.Items.Clear();
            foreach (var dto in dtos)
            {
                var item = new SalesOrderItem
                {
                    ItemId = dto.ItemId,
                    Unit = products[dto.ItemId].Unit,
                    UnitPrice = products[dto.ItemId].Price,
                    Quantity = dto.Quantity
                };
                order.AddItem(item);
            }
        }
    }
}
