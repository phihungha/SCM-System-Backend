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
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public SalesOrdersService(IMapper mapper, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TransOrderEventDto> AddManualEventAsync(int orderId, TransOrderEventCreateDto dto)
        {
            SalesOrder? order = await _dbContext.SalesOrders
                .Include(i => i.Events)
                .FirstOrDefaultAsync(i => i.Id == orderId);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            SalesOrderEvent orderEvent = order.AddManualEvent(dto.Type, dto.Location, dto.Message);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<TransOrderEventDto>(orderEvent);
        }

        public async Task<SalesOrderDto> CreateAsync(SalesOrderCreateDto dto, string userId)
        {
            Customer? customer = await _dbContext.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
            {
                throw new EntityNotFoundException("Customer not found");
            }

            var order = new SalesOrder
            {
                ToLocation = dto.ToLocation ?? customer.DefaultLocation,
                CustomerId = dto.CustomerId,
                CreateUserId = userId,
            };

            if (dto.ProductionFacilityId != null)
            {
                int facilityId = (int)dto.ProductionFacilityId;
                order.ProductionFacilityId = facilityId;
                order.FromLocation = await GetLocationOfProductionFacilityAsync(facilityId);
            }

            IList<SalesOrderItem> items = await AddOrderItemsFromDtos(dto.Items);
            order.AddItems(items);
            order.Begin(userId);

            _dbContext.SalesOrders.Add(order);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<SalesOrderDto>(order);
        }

        public async Task<SalesOrderDto?> GetAsync(int id)
        {
            SalesOrder? orders = await _dbContext.SalesOrders
                .Include(i => i.Items).ThenInclude(i => i.Product)
                .Include(i => i.Customer)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Events)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<SalesOrderDto?>(orders);
        }

        public async Task<IList<SalesOrderDto>> GetManyAsync()
        {
            IList<SalesOrder> orders = await _dbContext.SalesOrders
                .Include(i => i.Customer)
                .Include(i => i.ProductionFacility)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .ToListAsync();
            return _mapper.Map<IList<SalesOrderDto>>(orders);
        }

        public async Task<SalesOrderDto> UpdateAsync(
            int id,
            SalesOrderUpdateDto dto,
            string userId)
        {
            SalesOrder? order = await _dbContext.SalesOrders
                .Include(i => i.Items).ThenInclude(i => i.Product)
                .Include(i => i.Customer)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Events)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            if (dto.PaymentCompleted ?? false)
            {
                if (dto.PaymentAmount == null)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot complete payment without payment amount"
                        );
                }
                order.CompletePayment((decimal)dto.PaymentAmount);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<SalesOrderDto>(order);
            }

            if (!order.IsExecutionStarted)
            {
                if (dto.ProductionFacilityId != null)
                {
                    int facilityId = (int)dto.ProductionFacilityId;
                    order.ProductionFacilityId = facilityId;
                    order.FromLocation = await GetLocationOfProductionFacilityAsync(facilityId);
                }

                if (dto.Items != null)
                {
                    _dbContext.RemoveRange(order.Items);
                    IList<SalesOrderItem> items = await AddOrderItemsFromDtos(dto.Items);
                    order.AddItems(items);
                }
            }

            if (dto.ToLocation != null && !order.IsExecutionFinished)
            {
                order.ToLocation = dto.ToLocation ?? order.Customer.DefaultLocation;
            }

            switch (dto.Status)
            {
                case OrderStatusSelection.Executing:
                    order.StartExecution();
                    break;

                case OrderStatusSelection.WaitingAcceptance:
                    order.FinishExecution();
                    break;

                case OrderStatusSelection.Completed:
                    order.Complete(userId);
                    break;

                case OrderStatusSelection.Canceled:
                    if (dto.Problem == null)
                    {
                        throw new InvalidDomainOperationException(
                                "Cannot cancel an order without a problem."
                            );
                    }
                    order.Cancel(userId, dto.Problem);
                    break;

                case OrderStatusSelection.Returned:
                    if (dto.Problem == null)
                    {
                        throw new InvalidDomainOperationException(
                                "Cannot return an order without a problem."
                            );
                    }
                    order.Return(userId, dto.Problem);
                    break;
            }

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<SalesOrderDto>(order);
        }

        public async Task<TransOrderEventDto> UpdateEventAsync(int id, int orderId, OrderEventUpdateDto dto)
        {
            SalesOrder? order = await _dbContext.SalesOrders
                .Include(i => i.Events)
                .FirstOrDefaultAsync(i => i.Id == orderId);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            SalesOrderEvent orderEvent = order.UpdateEvent(id, dto.Location, dto.Message);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<TransOrderEventDto>(orderEvent);
        }

        private async Task<IList<SalesOrderItem>> AddOrderItemsFromDtos(IEnumerable<OrderItemInputDto> dtos)
        {
            IList<int> productIds = dtos.Select(i => i.ItemId).ToList();
            IDictionary<int, Product> products = await _dbContext
                .Products
                .Where(i => productIds.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id);

            return dtos.Select(
                dto => new SalesOrderItem
                {
                    ItemId = dto.ItemId,
                    Unit = products[dto.ItemId].Unit,
                    UnitPrice = products[dto.ItemId].Price,
                    Quantity = dto.Quantity
                }).ToList();
        }

        private async Task<string> GetLocationOfProductionFacilityAsync(int facilityId)
        {
            ProductionFacility? facility = await _dbContext
                .ProductionFacilities
                .FindAsync(facilityId);
            if (facility == null)
            {
                throw new EntityNotFoundException("Production facility not found");
            }
            return facility.Location;
        }
    }
}
