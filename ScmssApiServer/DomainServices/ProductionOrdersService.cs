using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class ProductionOrdersService : IProductionOrdersService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductionOrdersService(IMapper mapper, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProductionOrderEventDto> AddManualEventAsync(
            int orderId,
            ProductionOrderEventCreateDto dto)
        {
            ProductionOrder? order = await _dbContext.ProductionOrders
                .Include(i => i.Events)
                .FirstOrDefaultAsync(i => i.Id == orderId);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }
            ProductionOrderEvent item = order.AddManualEvent(dto.Type, dto.Location, dto.Message);
            await _dbContext.SaveChangesAsync();
            return GetEventDto(item);
        }

        public async Task<ProductionOrderDto> CreateAsync(ProductionOrderCreateDto dto, string userId)
        {
            ProductionFacility? facility = await _dbContext.ProductionFacilities.FindAsync(dto.ProductionFacilityId);
            if (facility == null)
            {
                throw new EntityNotFoundException("Production facility not found");
            }

            var item = new ProductionOrder
            {
                ProductionFacilityId = dto.ProductionFacilityId,
                CreateUserId = userId,
            };

            await AddOrderItemsFromDtos(item, dto.Items);
            item.Begin(userId);

            _dbContext.ProductionOrders.Add(item);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Entry(item).Reference(i => i.CreateUser).LoadAsync();
            return GetOrderDto(item);
        }

        public async Task<ProductionOrderDto?> GetAsync(int id)
        {
            ProductionOrder? item = await _dbContext.ProductionOrders
                .Include(i => i.Items).ThenInclude(i => i.Product)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Events)
                .Include(i => i.CreateUser)
                .Include(i => i.ApproveProductionManager)
                .Include(i => i.EndUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<ProductionOrderDto?>(item);
        }

        public async Task<IList<ProductionOrderDto>> GetManyAsync()
        {
            IList<ProductionOrder> items = await _dbContext.ProductionOrders
                .Include(i => i.ProductionFacility)
                .Include(i => i.CreateUser)
                .Include(i => i.ApproveProductionManager)
                .Include(i => i.EndUser)
                .ToListAsync();
            return _mapper.Map<IList<ProductionOrderDto>>(items);
        }

        public async Task<ProductionOrderDto> UpdateAsync(int id, ProductionOrderUpdateDto dto, string userId)
        {
            ProductionOrder? item = await _dbContext.ProductionOrders
                .Include(i => i.Items).ThenInclude(i => i.Product)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Events)
                .Include(i => i.CreateUser)
                .Include(i => i.ApproveProductionManager)
                .Include(i => i.EndUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (item == null)
            {
                throw new EntityNotFoundException();
            }

            if (dto.Items != null)
            {
                _dbContext.RemoveRange(item.Items);
                await AddOrderItemsFromDtos(item, dto.Items);
            }

            switch (dto.Status)
            {
                case OrderStatusSelection.Executing:
                    item.StartExecution();
                    break;

                case OrderStatusSelection.WaitingAcceptance:
                    item.FinishExecution();
                    break;

                case OrderStatusSelection.Completed:
                    item.Complete(userId);
                    break;

                case OrderStatusSelection.Canceled:
                    if (dto.Problem == null)
                    {
                        throw new InvalidDomainOperationException(
                                "Cannot cancel an order without a problem."
                            );
                    }
                    item.Cancel(userId, dto.Problem);
                    break;

                case OrderStatusSelection.Returned:
                    if (dto.Problem == null)
                    {
                        throw new InvalidDomainOperationException(
                                "Cannot return an order without a problem."
                            );
                    }
                    item.Return(userId, dto.Problem);
                    break;
            }

            switch (dto.ApprovalStatus)
            {
                case ApprovalStatus.Approved:
                    item.Approve(userId);
                    break;
                case ApprovalStatus.Rejected:
                    if (dto.Problem == null)
                    {
                        throw new InvalidDomainOperationException(
                                "Cannot reject an order without a problem."
                            );
                    }
                    item.Reject(userId, dto.Problem);
                    break;
            }

            await _dbContext.SaveChangesAsync();
            return GetOrderDto(item);
        }

        public async Task<ProductionOrderEventDto> UpdateEventAsync(int id, int orderId, OrderEventUpdateDto dto)
        {
            ProductionOrder? order = await _dbContext.ProductionOrders
                .Include(i => i.Events)
                .FirstOrDefaultAsync(i => i.Id == orderId);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            ProductionOrderEvent item = order.UpdateEvent(id, dto.Message, dto.Location);

            await _dbContext.SaveChangesAsync();
            return GetEventDto(item);
        }

        private async Task AddOrderItemsFromDtos(ProductionOrder order, IEnumerable<OrderItemInputDto> dtos)
        {
            IList<int> productIds = dtos.Select(i => i.ItemId).ToList();
            IDictionary<int, Product> products = await _dbContext
                .Products
                .Where(i => productIds.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id);

            order.Items.Clear();
            foreach (var dto in dtos)
            {
                var item = new ProductionOrderItem
                {
                    ItemId = dto.ItemId,
                    Unit = products[dto.ItemId].Unit,
                    UnitValue = products[dto.ItemId].Price,
                    UnitCost = products[dto.ItemId].Cost,
                    Quantity = dto.Quantity
                };
                order.AddItem(item);
            }
        }

        private ProductionOrderEventDto GetEventDto(ProductionOrderEvent item)
        {
            return _mapper.Map<ProductionOrderEventDto>(item);
        }

        private ProductionOrderDto GetOrderDto(ProductionOrder item)
        {
            return _mapper.Map<ProductionOrderDto>(item);
        }
    }
}
