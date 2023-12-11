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
        private readonly IConfigService _configService;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PurchaseRequisitionsService(IConfigService configService,
                                           AppDbContext dbContext,
                                           IMapper mapper,
                                           UserManager<User> userManager)
        {
            _configService = configService;
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<PurchaseRequisitionDto> CreateAsync(PurchaseRequisitionCreateDto dto, string userId)
        {
            User user = await _userManager.Users.Include(i => i.ProductionFacility)
                                                .FirstAsync(x => x.Id == userId);
            ProductionFacility? facility = user.ProductionFacility;
            if (facility == null)
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

            Config config = await _configService.GetAsync();

            var requisition = new PurchaseRequisition
            {
                VendorId = vendor.Id,
                Vendor = vendor,
                ProductionFacilityId = facility.Id,
                ProductionFacility = facility,
                CreateUserId = userId,
                CreateUser = user,
                VatRate = config.VatRate,
            };

            requisition.AddItems(await MapRequisitionItemDtosToModels(dto.VendorId, dto.Items));
            requisition.Begin(userId);

            _dbContext.PurchaseRequisitions.Add(requisition);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<PurchaseRequisitionDto>(requisition);
        }

        public async Task<PurchaseRequisitionDto?> GetAsync(int id)
        {
            PurchaseRequisition? requisition = await _dbContext.PurchaseRequisitions
                .AsNoTracking()
                .Include(i => i.Items)
                .ThenInclude(i => i.Supply)
                .Include(i => i.Vendor)
                .Include(i => i.ProductionFacility)
                .Include(i => i.PurchaseOrders)
                .Include(i => i.CreateUser)
                .Include(i => i.ApproveFinance)
                .Include(i => i.ApproveProductionManager)
                .Include(i => i.EndUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            return _mapper.Map<PurchaseRequisitionDto?>(requisition);
        }

        public async Task<IList<PurchaseRequisitionDto>> GetManyAsync()
        {
            IList<PurchaseRequisition> requisitions = await _dbContext.PurchaseRequisitions
                .AsNoTracking()
                .Include(i => i.ProductionFacility)
                .Include(i => i.Vendor)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .ToListAsync();
            return _mapper.Map<IList<PurchaseRequisitionDto>>(requisitions);
        }

        public async Task<PurchaseRequisitionDto> UpdateAsync(int id,
                                                              PurchaseRequisitionUpdateDto dto,
                                                              string userId)
        {
            PurchaseRequisition? requisition = await _dbContext.PurchaseRequisitions
                .Include(i => i.Items)
                .ThenInclude(i => i.Supply)
                .Include(i => i.Vendor)
                .Include(i => i.ProductionFacility)
                .Include(i => i.PurchaseOrders)
                .Include(i => i.CreateUser)
                .Include(i => i.ApproveFinance)
                .Include(i => i.ApproveProductionManager)
                .Include(i => i.EndUser)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (requisition == null)
            {
                throw new EntityNotFoundException();
            }

            if (dto.Items != null)
            {
                _dbContext.RemoveRange(requisition.Items);
                requisition.AddItems(
                        await MapRequisitionItemDtosToModels(requisition.VendorId, dto.Items)
                );
            }

            if (requisition.ApprovalStatus == ApprovalStatus.PendingApproval)
            {
                Config config = await _configService.GetAsync();
                requisition.VatRate = config.VatRate;
            }

            if (dto.IsCanceled ?? false)
            {
                if (dto.Problem == null)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot cancel a requisition without a problem."
                        );
                }
                requisition.Cancel(userId, dto.Problem);
            }

            if (dto.ApprovalStatus == ApprovalStatusOption.Approved)
            {
                requisition.Approve(userId);
            }
            else if (dto.ApprovalStatus == ApprovalStatusOption.Rejected)
            {
                if (dto.Problem == null)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot reject a requisition without a problem."
                        );
                }
                requisition.Reject(userId, dto.Problem);
            }

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<PurchaseRequisitionDto>(requisition);
        }

        private async Task<IList<PurchaseRequisitionItem>> MapRequisitionItemDtosToModels(
            int requisitionVendorId,
            IEnumerable<OrderItemInputDto> dtos)
        {
            IList<int> supplyIds = dtos.Select(i => i.ItemId).ToList();
            IDictionary<int, Supply> supplies = await _dbContext
                .Supplies
                .Where(i => i.VendorId == requisitionVendorId)
                .Where(i => supplyIds.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id);

            if (supplies.Count != supplyIds.Count)
            {
                throw new InvalidDomainOperationException(
                            "Cannot add a supply item with different vendor " +
                            "from the purchase requisition's vendor."
                        );
            }

            return dtos.Select(
                i => new PurchaseRequisitionItem
                {
                    ItemId = i.ItemId,
                    Supply = supplies[i.ItemId],
                    Unit = supplies[i.ItemId].Unit,
                    UnitPrice = supplies[i.ItemId].Price,
                    Quantity = i.Quantity
                }).ToList();
        }
    }
}
