using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.Exceptions;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.IServices;
using ScmssApiServer.Models;
using ScmssApiServer.Services;

namespace ScmssApiServer.DomainServices
{
    public class PurchaseOrdersService : IPurchaseOrdersService
    {
        private readonly IConfigService _configService;
        private readonly AppDbContext _dbContext;
        private readonly IFileHostService _fileHostService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PurchaseOrdersService(IConfigService configService,
                                     AppDbContext dbContext,
                                     IFileHostService fileHostService,
                                     IMapper mapper,
                                     UserManager<User> userManager)
        {
            _configService = configService;
            _dbContext = dbContext;
            _fileHostService = fileHostService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<TransOrderEventDto> AddManualEventAsync(
            int orderId,
            TransOrderEventCreateDto dto)
        {
            PurchaseOrder? order = await _dbContext.PurchaseOrders
                .Include(i => i.Events)
                .SingleOrDefaultAsync(i => i.Id == orderId);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            PurchaseOrderEvent orderEvent = order.AddManualEvent(dto.Type, dto.Location, dto.Message);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<TransOrderEventDto>(orderEvent);
        }

        public async Task<PurchaseOrderDto> CreateAsync(PurchaseOrderCreateDto dto, Identity identity)
        {
            PurchaseRequisition? requistion = await _dbContext
                .PurchaseRequisitions
                .Include(i => i.ProductionFacility)
                .Include(i => i.Vendor)
                .Include(i => i.Items).ThenInclude(i => i.Supply)
                .SingleOrDefaultAsync(i => i.Id == dto.PurchaseRequisitionId);
            if (requistion == null)
            {
                throw new EntityNotFoundException("Purchase requisition with provided ID not found.");
            }

            PurchaseOrder order = requistion.GeneratePurchaseOrder(identity.Id);

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

            if (dto.Items != null)
            {
                order.EditItemDiscounts(MapOrderItemDiscountDtosToDict(dto.Items));
            }

            User user = (await _userManager.FindByIdAsync(identity.Id))!;
            order.Begin(user);

            _dbContext.PurchaseOrders.Add(order);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Entry(order).Reference(i => i.CreateUser).LoadAsync();
            return _mapper.Map<PurchaseOrderDto>(order);
        }

        public string GenerateInvoiceUploadUrl(int id)
        {
            return _fileHostService.GenerateUploadUrl(PurchaseOrder.InvoiceFolderKey, id);
        }

        public string GenerateReceiptUploadUrl(int id)
        {
            return _fileHostService.GenerateUploadUrl(PurchaseOrder.ReceiptFolderKey, id);
        }

        public async Task<PurchaseOrderDto?> GetAsync(int id, Identity identity)
        {
            var query = _dbContext.PurchaseOrders.AsNoTracking();

            if (!identity.IsPurchaseUser && identity.IsInProductionFacility)
            {
                query = query.Where(i => i.ProductionFacilityId == identity.ProductionFacilityId);
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
                .SingleOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<PurchaseOrderDto?>(orders);
        }

        public async Task<IList<PurchaseOrderDto>> GetManyAsync(
            TransOrderQueryDto<PurchaseOrderSearchCriteria> dto,
            Identity identity)
        {
            PurchaseOrderSearchCriteria criteria = dto.SearchCriteria;
            string? searchTerm = dto.SearchTerm?.ToLower();
            ICollection<OrderStatus>? statuses = dto.Status;
            ICollection<TransOrderPaymentStatus>? paymentStatuses = dto.PaymentStatus;

            var query = _dbContext.PurchaseOrders.AsNoTracking();

            if (!identity.IsPurchaseUser && identity.IsInProductionFacility)
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
                    case PurchaseOrderSearchCriteria.CreateUserName:
                        query = query.Where(i => i.CreateUser.Name.ToLower().Contains(searchTerm));
                        break;

                    case PurchaseOrderSearchCriteria.VendorName:
                        query = query.Where(i => i.Vendor.Name.ToLower().Contains(searchTerm));
                        break;

                    case PurchaseOrderSearchCriteria.ProductionFacilityName:
                        query = query.Where(i => i.ProductionFacility != null &&
                                                 i.ProductionFacility.Name.ToLower().Contains(searchTerm));
                        break;

                    default:
                        query = query.Where(i => i.Id == int.Parse(searchTerm));
                        break;
                }
            }

            IList<PurchaseOrder> orders = await query
                .Include(i => i.Vendor)
                .Include(i => i.ProductionFacility)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .OrderBy(i => i.Id)
                .ToListAsync();
            return _mapper.Map<IList<PurchaseOrderDto>>(orders);
        }

        public async Task<PurchaseOrderDto> UpdateAsync(
            int id,
            PurchaseOrderUpdateDto dto,
            Identity identity)
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
                .SingleOrDefaultAsync(i => i.Id == id);
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

            if (dto.FromLocation != null)
            {
                if (!identity.IsPurchaseUser)
                {
                    throw new UnauthorizedException("Unauthorized to change location.");
                }
                order.FromLocation = dto.FromLocation;
            }

            if (dto.InvoiceName != null)
            {
                if (!identity.IsPurchaseUser)
                {
                    throw new UnauthorizedException("Unauthorized to change invoice.");
                }
                order.InvoiceName = dto.InvoiceName;
            }

            if (dto.ReceiptName != null)
            {
                if (!identity.IsPurchaseUser)
                {
                    throw new UnauthorizedException("Unauthorized to change invoice.");
                }
                order.ReceiptName = dto.ReceiptName;
            }

            if (dto.AdditionalDiscount != null)
            {
                if (!identity.IsPurchaseUser)
                {
                    throw new UnauthorizedException("Unauthorized to change additional discount.");
                }
                order.AdditionalDiscount = (decimal)dto.AdditionalDiscount;
            }

            if (dto.Items != null)
            {
                if (!identity.IsPurchaseUser)
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
                .SingleOrDefaultAsync(i => i.Id == orderId);
            if (order == null)
            {
                throw new EntityNotFoundException();
            }

            PurchaseOrderEvent orderEvent = order.UpdateEvent(id, dto.Location, dto.Message);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<TransOrderEventDto>(orderEvent);
        }

        private async Task ChangeStatusAsync(
            PurchaseOrder order,
            PurchaseOrderUpdateDto dto,
            Identity identity)
        {
            bool isInventoryStatus = dto.Status == OrderStatusOption.Completed ||
                                     dto.Status == OrderStatusOption.Returned;
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
                if (!identity.IsPurchaseUser)
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

        private void CompletePayment(PurchaseOrder order, PurchaseOrderUpdateDto dto, Identity identity)
        {
            if (!identity.IsFinanceUser)
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
