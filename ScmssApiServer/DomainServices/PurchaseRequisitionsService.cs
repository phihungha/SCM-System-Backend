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
    public class PurchaseRequisitionsService : IPurchaseRequisitionsService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PurchaseRequisitionsService(UserManager<User> userManager,
                                           IMapper mapper,
                                           AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<PurchaseRequisitionDto> CreateAsync(PurchaseRequisitionCreateDto dto, string userId)
        {
            User user = await _userManager.Users.Include(i => i.ProductionFacility)
                                                .FirstAsync(x => x.Id == userId);
            if (user.ProductionFacility == null)
            {
                throw new InvalidDomainOperationException(
                        "User needs to belong to a production facility " +
                        "to create a purchase requisition."
                    );
            }

            Vendor? vendor = await _dbContext.Vendors.FindAsync(dto.VendorId);
            if (vendor == null)
            {
                throw new EntityNotFoundException("Vendor not found.");
            }

            var item = new PurchaseRequisition
            {
                Vendor = vendor,
                ProductionFacility = user.ProductionFacility,
                CreateUserId = userId,
                CreateUser = user,
            };

            await AddOrderItemsFromDtos(item, dto.Items);
            item.Begin(userId);

            _dbContext.PurchaseRequisitions.Add(item);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<PurchaseRequisitionDto>(item);
        }

        public async Task<PurchaseRequisitionDto?> GetAsync(int id)
        {
            PurchaseRequisition? item = await _dbContext.PurchaseRequisitions
                .Include(i => i.Items).ThenInclude(i => i.Supply)
                .Include(i => i.Vendor)
                .Include(i => i.ProductionFacility)
                .Include(i => i.CreateUser)
                .Include(i => i.ApproveFinance)
                .Include(i => i.ApproveProductionManager)
                .Include(i => i.EndUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<PurchaseRequisitionDto?>(item);
        }

        public async Task<IList<PurchaseRequisitionDto>> GetManyAsync()
        {
            IList<PurchaseRequisition> items = await _dbContext.PurchaseRequisitions
                .Include(i => i.ProductionFacility)
                .Include(i => i.Vendor)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .ToListAsync();
            return _mapper.Map<IList<PurchaseRequisitionDto>>(items);
        }

        public async Task<PurchaseRequisitionDto> UpdateAsync(int id, PurchaseRequisitionUpdateDto dto, string userId)
        {
            PurchaseRequisition? item = await _dbContext.PurchaseRequisitions
                .Include(i => i.Items).ThenInclude(i => i.Supply)
                .Include(i => i.Vendor)
                .Include(i => i.ProductionFacility)
                .Include(i => i.CreateUser)
                .Include(i => i.ApproveFinance)
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

            User user = await _userManager.Users.FirstAsync(x => x.Id == userId);
            if (dto.IsCanceled ?? false)
            {
                if (dto.Problem == null)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot cancel a requisition without a problem."
                        );
                }
                item.Cancel(userId, dto.Problem);
            }

            if (dto.ApprovalStatus == ApprovalStatusOption.Approved)
            {
                item.Approve(userId);
            }
            else if (dto.ApprovalStatus == ApprovalStatusOption.Rejected)
            {
                if (dto.Problem == null)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot reject a requisition without a problem."
                        );
                }
                item.Reject(userId, dto.Problem);
            }

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<PurchaseRequisitionDto>(item);
        }

        private async Task AddOrderItemsFromDtos(PurchaseRequisition requisition, IEnumerable<OrderItemInputDto> dtos)
        {
            IList<int> supplyIds = dtos.Select(i => i.ItemId).ToList();
            IDictionary<int, Supply> supplies = await _dbContext
                .Supplies
                .Where(i => supplyIds.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id);

            IList<PurchaseRequisitionItem> requisitionItems = dtos.Select(
                i => new PurchaseRequisitionItem
                {
                    ItemId = i.ItemId,
                    Unit = supplies[i.ItemId].Unit,
                    UnitPrice = supplies[i.ItemId].Price,
                    Quantity = i.Quantity
                }).ToList();

            requisition.AddItems(requisitionItems);
        }
    }
}
