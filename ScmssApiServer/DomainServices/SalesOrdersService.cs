using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;
using ScmssApiServer.Services;

namespace ScmssApiServer.DomainServices
{
    public class SalesOrdersService : ISalesOrdersService
    {
        private readonly IConfigService _configService;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public SalesOrdersService(IConfigService configService,
                                  AppDbContext dbContext,
                                  IMapper mapper,
                                  UserManager<User> userManager)
        {
            _configService = configService;
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<TransOrderEventDto> AddManualEventAsync(int orderId,
                                                                  TransOrderEventCreateDto dto)
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

            Config config = await _configService.GetAsync();

            var order = new SalesOrder
            {
                ToLocation = dto.ToLocation ?? customer.DefaultLocation,
                CustomerId = dto.CustomerId,
                Customer = customer,
                CreateUserId = userId,
                VatRate = config.VatRate,
            };

            if (dto.ProductionFacilityId != null)
            {
                int facilityId = (int)dto.ProductionFacilityId;
                ProductionFacility facility = await GetProductionFacilityAsync(facilityId);
                order.ProductionFacilityId = facility.Id;
                order.ProductionFacility = facility;
                order.FromLocation = facility.Location;
            }

            order.AddItems(await MapOrderItemDtosToModels(dto.Items));
            order.Begin(userId);

            _dbContext.SalesOrders.Add(order);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Entry(order).Reference(i => i.CreateUser).LoadAsync();
            return _mapper.Map<SalesOrderDto>(order);
        }

        public async Task<SalesOrderDto?> GetAsync(int id, string userId)
        {
            var query = _dbContext.SalesOrders.AsNoTracking();

            User user = await _userManager.FindFullUserByIdAsync(userId);
            if (user.ProductionFacilityId != null)
            {
                query = query.Where(i => i.ProductionFacilityId == user.ProductionFacilityId);
            }

            SalesOrder? order = await query
                .AsNoTracking()
                .Include(i => i.Items)
                .ThenInclude(i => i.Product)
                .Include(i => i.Customer)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Events)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<SalesOrderDto?>(order);
        }

        public async Task<IList<SalesOrderDto>> GetManyAsync(string userId)
        {
            var query = _dbContext.SalesOrders.AsNoTracking();

            User user = await _userManager.FindFullUserByIdAsync(userId);
            if (user.ProductionFacilityId != null)
            {
                query = query.Where(i => i.ProductionFacilityId == user.ProductionFacilityId);
            }

            IList<SalesOrder> orders = await query
                .AsNoTracking()
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
                .Include(i => i.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(i => i.WarehouseProductItems)
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

            User user = await _userManager.FindFullUserByIdAsync(userId);

            if (order.PaymentStatus == TransOrderPaymentStatus.Pending)
            {
                Config config = await _configService.GetAsync();
                order.VatRate = config.VatRate;
            }

            if (dto.Status != null)
            {
                ChangeStatus(order, dto, user);
            }

            if (dto.PayAmount != null)
            {
                CompletePayment(order, dto, user);
            }

            if (dto.ToLocation != null)
            {
                if (!user.IsPurchaseUser)
                {
                    throw new UnauthorizedException("Unauthorized to change location.");
                }
                order.ToLocation = dto.ToLocation;
            }

            if (dto.ProductionFacilityId != null)
            {
                if (!user.IsPurchaseUser)
                {
                    throw new UnauthorizedException(
                            "Unauthorized to change production facility."
                        );
                }

                int facilityId = (int)dto.ProductionFacilityId;
                ProductionFacility facility = await GetProductionFacilityAsync(facilityId);
                order.ProductionFacilityId = facility.Id;
                order.ProductionFacility = facility;
                order.FromLocation = facility.Location;
            }

            if (dto.Items != null)
            {
                if (!user.IsPurchaseUser)
                {
                    throw new UnauthorizedException(
                            "Unauthorized to change items."
                        );
                }

                _dbContext.RemoveRange(order.Items);
                order.AddItems(await MapOrderItemDtosToModels(dto.Items));
            }

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<SalesOrderDto>(order);
        }

        public async Task<TransOrderEventDto> UpdateEventAsync(int id,
                                                               int orderId,
                                                               OrderEventUpdateDto dto)
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

        private void ChangeStatus(SalesOrder order, SalesOrderUpdateDto dto, User user)
        {
            bool isInventoryStatus = dto.Status == OrderStatusOption.Executing;
            if ((isInventoryStatus && !user.IsInventoryUser) ||
                (!isInventoryStatus && !user.IsSales))
            {
                throw new UnauthorizedException("Unauthorized for this status.");
            }

            string userId = user.Id;

            switch (dto.Status)
            {
                case OrderStatusOption.Executing:
                    order.StartExecution();
                    break;

                case OrderStatusOption.WaitingAcceptance:
                    order.FinishExecution();
                    break;

                case OrderStatusOption.Completed:
                    order.Complete(userId);
                    break;

                case OrderStatusOption.Canceled:
                    if (dto.Problem == null)
                    {
                        throw new InvalidDomainOperationException(
                                "Cannot cancel an order without a problem."
                            );
                    }
                    order.Cancel(userId, dto.Problem);
                    break;

                case OrderStatusOption.Returned:
                    if (dto.Problem == null)
                    {
                        throw new InvalidDomainOperationException(
                                "Cannot return an order without a problem."
                            );
                    }
                    order.Return(userId, dto.Problem);
                    break;
            }
        }

        private void CompletePayment(SalesOrder order, SalesOrderUpdateDto dto, User user)
        {
            if (!user.IsFinance)
            {
                throw new UnauthorizedException("Unauthorized to complete payment.");
            }
            order.CompletePayment((decimal)dto.PayAmount!);
        }

        private async Task<ProductionFacility> GetProductionFacilityAsync(int facilityId)
        {
            ProductionFacility? facility = await _dbContext.ProductionFacilities.FindAsync(facilityId);
            if (facility == null)
            {
                throw new EntityNotFoundException("Production facility not found.");
            }
            return facility;
        }

        private async Task<IList<SalesOrderItem>> MapOrderItemDtosToModels(IEnumerable<OrderItemInputDto> dtos)
        {
            IList<int> productIds = dtos.Select(i => i.ItemId).ToList();
            IDictionary<int, Product> products = await _dbContext
                .Products
                .Include(i => i.WarehouseProductItems)
                .Where(i => productIds.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id);

            return dtos.Select(
                dto => new SalesOrderItem
                {
                    ItemId = dto.ItemId,
                    // This is needed to check stock.
                    Product = products[dto.ItemId],
                    Unit = products[dto.ItemId].Unit,
                    UnitPrice = products[dto.ItemId].Price,
                    Quantity = dto.Quantity
                }).ToList();
        }
    }
}
