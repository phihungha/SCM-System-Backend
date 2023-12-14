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

        public async Task<SalesOrderDto> CreateAsync(SalesOrderCreateDto dto, Identity identity)
        {
            Customer? customer = await _dbContext.Customers.Where(i => i.IsActive)
                                                           .FirstOrDefaultAsync(i => i.Id == dto.CustomerId);
            if (customer == null)
            {
                throw new EntityNotFoundException("Customer not found");
            }

            var order = new SalesOrder
            {
                ToLocation = dto.ToLocation ?? customer.DefaultLocation,
                CustomerId = customer.Id,
                Customer = customer,
                CreateUserId = identity.Id,
            };

            if (dto.ProductionFacilityId != null)
            {
                int facilityId = (int)dto.ProductionFacilityId;
                ProductionFacility facility = await GetProductionFacilityAsync(facilityId);
                order.ProductionFacilityId = facility.Id;
                order.ProductionFacility = facility;
                order.FromLocation = facility.Location;
            }

            Config config = await _configService.GetAsync();
            order.VatRate = config.VatRate;

            order.AddItems(await MapOrderItemDtosToModels(dto.Items));

            User user = (await _userManager.FindByIdAsync(identity.Id))!;
            order.Begin(user);

            _dbContext.SalesOrders.Add(order);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<SalesOrderDto>(order);
        }

        public async Task<SalesOrderDto?> GetAsync(int id, Identity identity)
        {
            var query = _dbContext.SalesOrders.AsNoTracking();

            if (!identity.IsSalesUser && identity.IsInProductionFacility)
            {
                query = query.Where(i => i.ProductionFacilityId == identity.ProductionFacilityId);
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

        public async Task<IList<SalesOrderDto>> GetManyAsync(
            TransOrderQueryDto<SalesOrderSearchCriteria> dto,
            Identity identity)
        {
            SalesOrderSearchCriteria criteria = dto.SearchCriteria;
            string? searchTerm = dto.SearchTerm?.ToLower();
            ICollection<OrderStatus>? statuses = dto.Status;
            ICollection<TransOrderPaymentStatus>? paymentStatuses = dto.PaymentStatus;

            var query = _dbContext.SalesOrders.AsNoTracking();

            if (!identity.IsSalesUser && identity.IsInProductionFacility)
            {
                query = query.Where(i => i.ProductionFacilityId == identity.ProductionFacilityId);
            }

            if (statuses != null)
            {
                query = query.Where(i => statuses.Contains(i.Status));
            }

            if (paymentStatuses != null)
            {
                query = query.Where(i => paymentStatuses.Contains(i.PaymentStatus));
            }

            if (searchTerm != null)
            {
                switch (criteria)
                {
                    case SalesOrderSearchCriteria.CreateUserName:
                        query = query.Where(i => i.CreateUser.Name.ToLower().Contains(searchTerm));
                        break;

                    case SalesOrderSearchCriteria.CustomerName:
                        query = query.Where(i => i.Customer.Name.ToLower().Contains(searchTerm));
                        break;

                    case SalesOrderSearchCriteria.ProductionFacilityName:
                        query = query.Where(i => i.ProductionFacility != null &&
                                                 i.ProductionFacility.Name.ToLower().Contains(searchTerm));
                        break;

                    default:
                        query = query.Where(i => i.Id == int.Parse(searchTerm));
                        break;
                }
            }

            IList<SalesOrder> orders = await query
                .Include(i => i.Customer)
                .Include(i => i.ProductionFacility)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .OrderBy(i => i.Id)
                .ToListAsync();
            return _mapper.Map<IList<SalesOrderDto>>(orders);
        }

        public async Task<SalesOrderDto> UpdateAsync(
            int id,
            SalesOrderUpdateDto dto,
            Identity identity)
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

            if (dto.Status != null)
            {
                await ChangeStatusAsync(order, dto, identity);
            }

            if (dto.PayAmount != null)
            {
                CompletePayment(order, dto, identity);
            }

            if (dto.ToLocation != null)
            {
                if (!identity.IsSalesUser)
                {
                    throw new UnauthorizedException("Unauthorized to change location.");
                }
                order.ToLocation = dto.ToLocation;
            }

            if (dto.ProductionFacilityId != null)
            {
                if (!identity.IsSalesUser)
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
                if (!identity.IsSalesUser)
                {
                    throw new UnauthorizedException(
                            "Unauthorized to change items."
                        );
                }

                _dbContext.RemoveRange(order.Items);
                order.AddItems(await MapOrderItemDtosToModels(dto.Items));
            }

            if (order.PaymentStatus == TransOrderPaymentStatus.Pending)
            {
                Config config = await _configService.GetAsync();
                order.VatRate = config.VatRate;
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

        private async Task ChangeStatusAsync(
            SalesOrder order,
            SalesOrderUpdateDto dto,
            Identity identity)
        {
            bool isInventoryStatus = dto.Status == OrderStatusOption.Executing;
            if (isInventoryStatus)
            {
                if (!identity.IsInventoryUser)
                {
                    throw new UnauthorizedException("Unauthorized for this status.");
                }

                if (!identity.IsSuperUser && identity.ProductionFacilityId != order.ProductionFacilityId)
                {
                    throw new UnauthorizedException("Unauthorized to handle sales orders of another facility.");
                }
            }
            else
            {
                if (!identity.IsSalesUser)
                {
                    throw new UnauthorizedException("Unauthorized for this status.");
                }
            }

            User user = (await _userManager.FindByIdAsync(identity.Id))!;

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
                    order.Complete(user);
                    break;

                case OrderStatusOption.Canceled:
                    if (dto.Problem == null)
                    {
                        throw new InvalidDomainOperationException(
                                "Cannot cancel an order without a problem."
                            );
                    }
                    order.Cancel(user, dto.Problem);
                    break;

                case OrderStatusOption.Returned:
                    if (dto.Problem == null)
                    {
                        throw new InvalidDomainOperationException(
                                "Cannot return an order without a problem."
                            );
                    }
                    order.Return(user, dto.Problem);
                    break;
            }
        }

        private void CompletePayment(SalesOrder order, SalesOrderUpdateDto dto, Identity identity)
        {
            if (!identity.IsFinanceUser)
            {
                throw new UnauthorizedException("Unauthorized to complete payment.");
            }
            order.CompletePayment((decimal)dto.PayAmount!);
        }

        private async Task<ProductionFacility> GetProductionFacilityAsync(int facilityId)
        {
            ProductionFacility? facility = await _dbContext.ProductionFacilities.Where(i => i.IsActive)
                                                                                .FirstOrDefaultAsync(i => i.Id == facilityId);
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
                .Where(i => i.IsActive)
                .ToDictionaryAsync(i => i.Id);

            var orderItems = new List<SalesOrderItem>();

            foreach (var dto in dtos)
            {
                int itemId = dto.ItemId;
                if (!products.ContainsKey(itemId))
                {
                    throw new InvalidDomainOperationException($"Product item {itemId} not found.");
                }
                Product product = products[itemId];

                orderItems.Add(new SalesOrderItem
                {
                    ItemId = itemId,
                    // This is needed to check stock.
                    Product = product,
                    Unit = product.Unit,
                    UnitPrice = product.Price,
                    Quantity = dto.Quantity
                });
            }

            return orderItems;
        }
    }
}
