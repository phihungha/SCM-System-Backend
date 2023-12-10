using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class PurchaseOrdersService : IPurchaseOrdersService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public PurchaseOrdersService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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

            if (dto.FromLocation != null)
            {
                order.FromLocation = dto.FromLocation;
            }

            if (dto.AdditionalDiscount != null)
            {
                order.AdditionalDiscount = (decimal)dto.AdditionalDiscount;
            }

            order.EditItemDiscounts(MapOrderItemDtosToDict(dto.Items));
            order.Begin(userId);

            _dbContext.PurchaseOrders.Add(order);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Entry(order).Reference(i => i.CreateUser).LoadAsync();
            return _mapper.Map<PurchaseOrderDto>(order);
        }

        public async Task<PurchaseOrderDto?> GetAsync(int id)
        {
            PurchaseOrder? orders = await _dbContext.PurchaseOrders
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

        public async Task<IList<PurchaseOrderDto>> GetManyAsync()
        {
            IList<PurchaseOrder> orders = await _dbContext.PurchaseOrders
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
                .Include(i => i.Items).ThenInclude(i => i.Supply)
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

            if (dto.PaymentAmount != null)
            {
                order.CompletePayment((decimal)dto.PaymentAmount);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<PurchaseOrderDto>(order);
            }

            if (dto.FromLocation != null)
            {
                order.FromLocation = dto.FromLocation;
            }

            if (dto.Items != null)
            {
                order.EditItemDiscounts(MapOrderItemDtosToDict(dto.Items));
            }

            if (dto.AdditionalDiscount != null)
            {
                order.AdditionalDiscount = (decimal)dto.AdditionalDiscount;
            }

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

        private IDictionary<int, decimal> MapOrderItemDtosToDict(
            IEnumerable<PurchaseOrderItemInputDto> dtos)
        {
            return dtos.ToDictionary(i => i.ItemId, i => i.Discount);
        }
    }
}
