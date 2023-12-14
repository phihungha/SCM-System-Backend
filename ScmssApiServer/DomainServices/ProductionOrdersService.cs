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
    public class ProductionOrdersService : IProductionOrdersService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ProductionOrdersService(AppDbContext dbContext,
                                       IMapper mapper,
                                       UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
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

            ProductionOrderEvent orderEvent = order.AddManualEvent(dto.Type, dto.Location, dto.Message);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProductionOrderEventDto>(orderEvent);
        }

        public async Task<ProductionOrderDto> CreateAsync(
            OrderCreateDto<OrderItemInputDto> dto,
            Identity identity)
        {
            if (!identity.IsInProductionFacility)
            {
                throw new InvalidDomainOperationException(
                        "User must belong to a production facility " +
                        "to create a production order."
                    );
            }

            User user = await _userManager.Users.Include(i => i.ProductionFacility)
                                                .FirstAsync(i => i.Id == identity.Id);
            ProductionFacility facility = user.ProductionFacility!;

            var order = new ProductionOrder
            {
                ProductionFacilityId = facility.Id,
                ProductionFacility = facility,
                CreateUserId = user.Id,
                CreateUser = user,
            };

            order.AddItems(await MapOrderItemDtosToModels(dto.Items));
            order.Begin(user.Id);

            _dbContext.ProductionOrders.Add(order);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProductionOrderDto>(order);
        }

        public async Task<ProductionOrderDto?> GetAsync(int id, Identity identity)
        {
            var query = _dbContext.ProductionOrders.AsNoTracking();

            if (!identity.IsSuperUser && identity.IsInProductionFacility)
            {
                query = query.Where(i => i.ProductionFacilityId == identity.ProductionFacilityId);
            }

            ProductionOrder? order = await query
                .Include(i => i.Items).ThenInclude(i => i.Product)
                .Include(i => i.SupplyUsageItems)
                .ThenInclude(i => i.Supply)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Events)
                .Include(i => i.CreateUser)
                .Include(i => i.ApproveProductionManager)
                .Include(i => i.EndUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<ProductionOrderDto?>(order);
        }

        public async Task<IList<ProductionOrderDto>> GetManyAsync(ProductionOrderQueryDto dto, Identity identity)
        {
            ProductionOrderSearchCriteria criteria = dto.SearchCriteria;
            string? searchTerm = dto.SearchTerm?.ToLower();
            ICollection<OrderStatus>? statuses = dto.Status;
            ICollection<ApprovalStatus>? approvalStatuses = dto.ApprovalStatus;

            var query = _dbContext.ProductionOrders.AsNoTracking();

            if (!identity.IsSuperUser && identity.IsInProductionFacility)
            {
                query = query.Where(i => i.ProductionFacilityId == identity.ProductionFacilityId);
            }

            if (statuses != null)
            {
                query = query.Where(i => statuses.Contains(i.Status));
            }

            if (approvalStatuses != null)
            {
                query = query.Where(i => approvalStatuses.Contains(i.ApprovalStatus));
            }

            if (searchTerm != null)
            {
                switch (criteria)
                {
                    case ProductionOrderSearchCriteria.CreateUserName:
                        query = query.Where(i => i.CreateUser.Name.ToLower().Contains(searchTerm));
                        break;

                    case ProductionOrderSearchCriteria.ProductionFacilityName:
                        query = query.Where(i => i.ProductionFacility != null &&
                                                 i.ProductionFacility.Name.ToLower().Contains(searchTerm));
                        break;

                    default:
                        query = query.Where(i => i.Id == int.Parse(searchTerm));
                        break;
                }
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

        public async Task<ProductionOrderDto> UpdateAsync(
            int id,
            ProductionOrderUpdateDto dto,
            Identity identity)
        {
            ProductionOrder? order = await _dbContext.ProductionOrders
                .Include(i => i.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(i => i.SupplyCostItems)
                .ThenInclude(i => i.Supply)
                .ThenInclude(i => i.WarehouseSupplyItems)
                .Include(i => i.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(i => i.WarehouseProductItems)
                .Include(i => i.SupplyUsageItems)
                .Include(i => i.ProductionFacility)
                .Include(i => i.Events)
                .Include(i => i.CreateUser)
                .Include(i => i.ApproveProductionManager)
                .Include(i => i.EndUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            if (!identity.IsSuperUser &&
                identity.IsInProductionFacility &&
                identity.ProductionFacilityId != order.ProductionFacilityId)
            {
                throw new UnauthorizedException(
                        "Unauthorized to handle production orders of another facility."
                    );
            }

            if (dto.Status != null)
            {
                await ChangeStatusAsync(order, dto, identity);
            }

            if (dto.ApprovalStatus != null)
            {
                await HandleApprovalAsync(order, dto, identity);
            }

            if (dto.Items != null)
            {
                if (!identity.IsProductionUser)
                {
                    throw new UnauthorizedException("Unauthorized to change items.");
                }

                _dbContext.RemoveRange(order.Items);
                _dbContext.RemoveRange(order.SupplyUsageItems);
                order.AddItems(await MapOrderItemDtosToModels(dto.Items));
            }

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProductionOrderDto>(order);
        }

        public async Task<ProductionOrderEventDto> UpdateEventAsync(
            int id,
            int orderId,
            OrderEventUpdateDto dto)
        {
            ProductionOrder? order = await _dbContext.ProductionOrders
                .Include(i => i.Events)
                .FirstOrDefaultAsync(i => i.Id == orderId);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            ProductionOrderEvent orderEvent = order.UpdateEvent(id, dto.Message, dto.Location);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProductionOrderEventDto>(orderEvent);
        }

        private async Task ChangeStatusAsync(
            ProductionOrder order,
            ProductionOrderUpdateDto dto,
            Identity identity)
        {
            bool isInventoryStatus = dto.Status == OrderStatusOption.Executing ||
                                     dto.Status == OrderStatusOption.Completed ||
                                     dto.Status == OrderStatusOption.Returned;
            if ((isInventoryStatus && !identity.IsInventoryUser) ||
                (!isInventoryStatus && !identity.IsProductionUser))
            {
                throw new UnauthorizedException("Unauthorized for this status.");
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

        private async Task HandleApprovalAsync(
            ProductionOrder order,
            ProductionOrderUpdateDto dto,
            Identity identity)
        {
            if (!identity.Roles.Contains("ProductionManager"))
            {
                throw new UnauthorizedException("Not authorized to handle approval.");
            }

            User user = (await _userManager.FindByIdAsync(identity.Id))!;

            if (dto.ApprovalStatus == ApprovalStatusOption.Approved)
            {
                order.Approve(user);
            }
            else if (dto.ApprovalStatus == ApprovalStatusOption.Rejected)
            {
                if (dto.Problem == null)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot reject a production order without a problem."
                        );
                }
                order.Reject(user, dto.Problem);
            }
        }

        private async Task<IList<ProductionOrderItem>> MapOrderItemDtosToModels(
            IEnumerable<OrderItemInputDto> dtos)
        {
            IList<int> productIds = dtos.Select(i => i.ItemId).ToList();
            IDictionary<int, Product> products = await _dbContext
                .Products
                .Include(i => i.SupplyCostItems)
                .ThenInclude(i => i.Supply)
                .ThenInclude(i => i.WarehouseSupplyItems)
                .Where(i => productIds.Contains(i.Id))
                .Where(i => i.IsActive)
                .ToDictionaryAsync(i => i.Id);

            var orderItems = new List<ProductionOrderItem>();

            foreach (var dto in dtos)
            {
                int itemId = dto.ItemId;
                if (!products.ContainsKey(itemId))
                {
                    throw new InvalidDomainOperationException($"Product item {itemId} not found.");
                }
                Product product = products[itemId];

                orderItems.Add(new ProductionOrderItem
                {
                    ItemId = itemId,
                    // This is needed to check stock.
                    Product = product,
                    Unit = product.Unit,
                    UnitValue = product.Price,
                    UnitCost = product.Cost,
                    Quantity = dto.Quantity
                });
            }

            return orderItems;
        }
    }
}
