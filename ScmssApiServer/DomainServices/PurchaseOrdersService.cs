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
    public class PurchaseOrdersService : IPurchaseOrdersService
    {
        private readonly IConfigService _configService;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PurchaseOrdersService(AppDbContext dbContext,
                                     IConfigService configService,
                                     IMapper mapper,
                                     UserManager<User> userManager)
        {
            _configService = configService;
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<TransOrderEventDto> AddManualEventAsync(
            int orderId,
            TransOrderEventCreateDto dto)
        {
            PurchaseOrder? order = await _dbContext.PurchaseOrders
                .Include(i => i.Events)
                .FirstOrDefaultAsync(i => i.Id == orderId);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            PurchaseOrderEvent orderEvent = order.AddManualEvent(dto.Type, dto.Location, dto.Message);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<TransOrderEventDto>(orderEvent);
        }

        public async Task<PurchaseOrderDto> CreateAsync(PurchaseOrderCreateDto dto, string userId)
        {
            PurchaseRequisition? requistion = await _dbContext
                .PurchaseRequisitions
                .Include(i => i.ProductionFacility)
                .Include(i => i.Vendor)
                .Include(i => i.Items).ThenInclude(i => i.Supply)
                .FirstOrDefaultAsync(i => i.Id == dto.PurchaseRequisitionId);
            if (requistion == null)
            {
                throw new EntityNotFoundException("Purchase requisition with provided ID not found.");
            }

            PurchaseOrder order = requistion.GeneratePurchaseOrder(userId);

            Config config = await _configService.GetAsync();
            order.VatRate = config.VatRate;

            if (dto.FromLocation != null)
            {
                order.FromLocation = dto.FromLocation;
            }

            if (dto.AdditionalDiscount != null)
            {
                order.AdditionalDiscount = (decimal)dto.AdditionalDiscount;
            }

            order.EditItemDiscounts(MapOrderItemDiscountDtosToDict(dto.Items));
            order.Begin(userId);

            _dbContext.PurchaseOrders.Add(order);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Entry(order).Reference(i => i.CreateUser).LoadAsync();
            return _mapper.Map<PurchaseOrderDto>(order);
        }

        public async Task<PurchaseOrderDto?> GetAsync(int id, string userId)
        {
            var query = _dbContext.PurchaseOrders.AsNoTracking();

            User user = await _userManager.FindFullUserByIdAsync(userId);
            if (user.IsInProductionFacility)
            {
                query = query.Where(i => i.ProductionFacilityId == user.ProductionFacilityId);
            }

            PurchaseOrder? orders = await query
                .AsNoTracking()
                .Include(i => i.Items).ThenInclude(i => i.Supply)
                .Include(i => i.Vendor)
                .Include(i => i.ProductionFacility)
                .Include(i => i.PurchaseRequisition)
                .Include(i => i.Events)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<PurchaseOrderDto?>(orders);
        }

        public async Task<IList<PurchaseOrderDto>> GetManyAsync(string userId)
        {
            var query = _dbContext.PurchaseOrders.AsNoTracking();

            User user = await _userManager.FindFullUserByIdAsync(userId);
            if (user.IsInProductionFacility)
            {
                query = query.Where(i => i.ProductionFacilityId == user.ProductionFacilityId);
            }

            IList<PurchaseOrder> orders = await query
                .AsNoTracking()
                .Include(i => i.Vendor)
                .Include(i => i.ProductionFacility)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .ToListAsync();
            return _mapper.Map<IList<PurchaseOrderDto>>(orders);
        }

        public async Task<PurchaseOrderDto> UpdateAsync(
            int id,
            PurchaseOrderUpdateDto dto,
            string userId)
        {
            PurchaseOrder? order = await _dbContext.PurchaseOrders
                .Include(i => i.Items)
                .ThenInclude(i => i.Supply)
                .ThenInclude(i => i.WarehouseSupplyItems)
                .Include(i => i.Vendor)
                .Include(i => i.ProductionFacility)
                .Include(i => i.PurchaseRequisition)
                .Include(i => i.Events)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            User user = await _userManager.FindFullUserByIdAsync(userId);

            if (dto.Status != null)
            {
                ChangeStatus(order, dto, user);
            }

            if (dto.PayAmount != null)
            {
                CompletePayment(order, dto, user);
            }

            if (dto.InvoiceUrl != null)
            {
                if (!user.IsPurchaseUser)
                {
                    throw new UnauthorizedException("Unauthorized to change invoice URL.");
                }
                order.InvoiceUrl = dto.InvoiceUrl;
            }

            if (dto.ReceiptUrl != null)
            {
                if (!user.IsPurchaseUser)
                {
                    throw new UnauthorizedException("Unauthorized to change receipt URL.");
                }
                order.ReceiptUrl = dto.ReceiptUrl;
            }

            if (dto.FromLocation != null)
            {
                if (!user.IsPurchaseUser)
                {
                    throw new UnauthorizedException("Unauthorized to change location.");
                }
                order.FromLocation = dto.FromLocation;
            }

            if (dto.AdditionalDiscount != null)
            {
                if (!user.IsPurchaseUser)
                {
                    throw new UnauthorizedException("Unauthorized to change additional discount.");
                }
                order.AdditionalDiscount = (decimal)dto.AdditionalDiscount;
            }

            if (dto.Items != null)
            {
                if (!user.IsPurchaseUser)
                {
                    throw new UnauthorizedException("Unauthorized to change items.");
                }
                order.EditItemDiscounts(MapOrderItemDiscountDtosToDict(dto.Items));
            }

            if (order.PaymentStatus == TransOrderPaymentStatus.Pending)
            {
                Config config = await _configService.GetAsync();
                order.VatRate = config.VatRate;
            }

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<PurchaseOrderDto>(order);
        }

        public async Task<TransOrderEventDto> UpdateEventAsync(
            int id,
            int orderId,
            OrderEventUpdateDto dto)
        {
            PurchaseOrder? order = await _dbContext.PurchaseOrders
                .Include(i => i.Events)
                .FirstOrDefaultAsync(i => i.Id == orderId);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            PurchaseOrderEvent orderEvent = order.UpdateEvent(id, dto.Location, dto.Message);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<TransOrderEventDto>(orderEvent);
        }

        private void ChangeStatus(PurchaseOrder order,
                                  PurchaseOrderUpdateDto dto,
                                  User user)
        {
            bool isInventoryStatus = dto.Status == OrderStatusOption.Completed ||
                                     dto.Status == OrderStatusOption.Returned;
            if ((isInventoryStatus && !user.IsInventoryUser) ||
                (!isInventoryStatus && !user.IsPurchaseUser))
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

        private void CompletePayment(PurchaseOrder order, PurchaseOrderUpdateDto dto, User user)
        {
            if (!user.IsFinanceUser)
            {
                throw new UnauthorizedException("Unauthorized to complete payment.");
            }
            order.CompletePayment((decimal)dto.PayAmount!);
        }

        private IDictionary<int, decimal> MapOrderItemDiscountDtosToDict(
            IEnumerable<PurchaseOrderItemDiscountInputDto> dtos)
        {
            return dtos.ToDictionary(i => i.ItemId, i => i.Discount);
        }
    }
}
