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
            string userId)
        {
            User user = await _userManager.Users.Include(i => i.ProductionFacility)
                                                .FirstAsync(i => i.Id == userId);
            if (user.ProductionFacility == null)
            {
                throw new InvalidDomainOperationException(
                        "User must belong to a production facility " +
                        "to create a production order."
                    );
            }

            var order = new ProductionOrder
            {
                ProductionFacilityId = user.ProductionFacility.Id,
                ProductionFacility = user.ProductionFacility,
                CreateUserId = userId,
                CreateUser = user,
            };

            order.AddItems(await MapOrderItemDtosToModels(dto.Items));
            order.Begin(userId);

            _dbContext.ProductionOrders.Add(order);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProductionOrderDto>(order);
        }

        public async Task<ProductionOrderDto?> GetAsync(int id, string userId)
        {
            var query = _dbContext.ProductionOrders.AsNoTracking();

            User user = await _userManager.FindFullUserByIdAsync(userId);
            if (user.IsInProductionFacility)
            {
                query = query.Where(i => i.ProductionFacilityId == user.ProductionFacilityId);
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

        public async Task<IList<ProductionOrderDto>> GetManyAsync(string userId)
        {
            var query = _dbContext.ProductionOrders.AsNoTracking();

            User user = await _userManager.FindFullUserByIdAsync(userId);
            if (user.IsInProductionFacility)
            {
                query = query.Where(i => i.ProductionFacilityId == user.ProductionFacilityId);
            }

            IList<ProductionOrder> orders = await query
                .Include(i => i.ProductionFacility)
                .Include(i => i.CreateUser)
                .Include(i => i.ApproveProductionManager)
                .Include(i => i.EndUser)
                .ToListAsync();
            return _mapper.Map<IList<ProductionOrderDto>>(orders);
        }

        public async Task<ProductionOrderDto> UpdateAsync(
            int id,
            ProductionOrderUpdateDto dto,
            string userId)
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

            User user = await _userManager.FindFullUserByIdAsync(userId);

            if (user.IsInProductionFacility && user.ProductionFacilityId != order.ProductionFacilityId)
            {
                throw new UnauthorizedException(
                        "Unauthorized to handle production orders of another facility."
                    );
            }

            if (dto.Status != null)
            {
                ChangeStatus(order, dto, user);
            }

            if (dto.ApprovalStatus != null)
            {
                HandleApproval(order, dto, user);
            }

            if (dto.Items != null)
            {
                if (!user.IsProductionUser)
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

        private void ChangeStatus(ProductionOrder order,
                                        ProductionOrderUpdateDto dto,
                                        User user)
        {
            bool isInventoryStatus = dto.Status == OrderStatusOption.Executing ||
                                     dto.Status == OrderStatusOption.Completed ||
                                     dto.Status == OrderStatusOption.Returned;
            if ((isInventoryStatus && !user.IsInventoryUser) ||
                (!isInventoryStatus && !user.IsProductionUser))
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

        private void HandleApproval(ProductionOrder order,
                                    ProductionOrderUpdateDto dto,
                                    User user)
        {
            if (!user.Roles.Contains("ProductionManager"))
            {
                throw new UnauthorizedException("Not authorized to handle approval.");
            }

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
                .ToDictionaryAsync(i => i.Id);

            return dtos.Select(
                dto => new ProductionOrderItem
                {
                    ItemId = dto.ItemId,
                    // This is needed to check stock.
                    Product = products[dto.ItemId],
                    Unit = products[dto.ItemId].Unit,
                    UnitValue = products[dto.ItemId].Price,
                    UnitCost = products[dto.ItemId].Cost,
                    Quantity = dto.Quantity
                }).ToList();
        }
    }
}
