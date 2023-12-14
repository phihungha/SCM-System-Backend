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

        public async Task<PurchaseRequisitionDto> CreateAsync(
            PurchaseRequisitionCreateDto dto,
            Identity identity)
        {
            if (!identity.IsInProductionFacility)
            {
                throw new InvalidDomainOperationException(
                        "User must belong to a production facility " +
                        "to create a purchase requisition."
                    );
            }

            Vendor? vendor = await _dbContext.Vendors.FindAsync(dto.VendorId);
            if (vendor == null)
            {
                throw new EntityNotFoundException("Vendor not found.");
            }

            Config config = await _configService.GetAsync();

            User user = await _userManager.Users.Include(i => i.ProductionFacility)
                                                .FirstAsync(i => i.Id == identity.Id);
            ProductionFacility facility = user.ProductionFacility!;

            var requisition = new PurchaseRequisition
            {
                VendorId = vendor.Id,
                Vendor = vendor,
                ProductionFacilityId = facility.Id,
                ProductionFacility = facility,
                CreateUserId = user.Id,
                CreateUser = user,
                VatRate = config.VatRate,
            };

            requisition.AddItems(await MapRequisitionItemDtosToModels(dto.VendorId, dto.Items));
            requisition.Begin(user.Id);

            _dbContext.PurchaseRequisitions.Add(requisition);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<PurchaseRequisitionDto>(requisition);
        }

        public async Task<PurchaseRequisitionDto?> GetAsync(int id, Identity identity)
        {
            var query = _dbContext.PurchaseRequisitions.AsNoTracking();

            if (!identity.IsSuperUser && identity.IsInProductionFacility)
            {
                query = query.Where(i => i.ProductionFacilityId == identity.ProductionFacilityId);
            }

            PurchaseRequisition? requisition = await query
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

        public async Task<IList<PurchaseRequisitionDto>> GetManyAsync(PurchaseRequisitionQueryDto dto, Identity identity)
        {
            PurchaseRequisitionSearchCriteria? criteria = dto.SearchCriteria;
            string? searchTerm = dto.SearchTerm?.ToLower();
            ICollection<PurchaseRequisitionStatus>? statuses = dto.Status;
            ICollection<ApprovalStatus>? approvalStatuses = dto.ApprovalStatus;

            var query = _dbContext.PurchaseRequisitions.AsNoTracking();

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
                    case PurchaseRequisitionSearchCriteria.CreateUserName:
                        query = query.Where(i => i.CreateUser.Name.ToLower().Contains(searchTerm));
                        break;

                    case PurchaseRequisitionSearchCriteria.VendorName:
                        query = query.Where(i => i.Vendor.Name.ToLower().Contains(searchTerm));
                        break;

                    case PurchaseRequisitionSearchCriteria.ProductionFacilityName:
                        query = query.Where(i => i.ProductionFacility != null &&
                                                 i.ProductionFacility.Name.ToLower().Contains(searchTerm));
                        break;

                    default:
                        query = query.Where(i => i.Id == int.Parse(searchTerm));
                        break;
                }
            }

            IList<PurchaseRequisition> requisitions = await query
                .Include(i => i.ProductionFacility)
                .Include(i => i.Vendor)
                .Include(i => i.CreateUser)
                .Include(i => i.EndUser)
                .OrderBy(i => i.Id)
                .ToListAsync();
            return _mapper.Map<IList<PurchaseRequisitionDto>>(requisitions);
        }

        public async Task<PurchaseRequisitionDto> UpdateAsync(int id,
                                                              PurchaseRequisitionUpdateDto dto,
                                                              Identity identity)
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

            if (!identity.IsSuperUser &&
                identity.IsInProductionFacility &&
                identity.ProductionFacilityId != requisition.ProductionFacilityId)
            {
                throw new UnauthorizedException(
                        "Unauthorized to handle purchase requisitions of another facility."
                    );
            }

            if (dto.IsCanceled ?? false)
            {
                if (!identity.IsProductionUser)
                {
                    throw new UnauthorizedException("Unauthorized to cancel.");
                }

                if (dto.Problem == null)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot cancel a requisition without a problem."
                        );
                }

                User user = (await _userManager.FindByIdAsync(identity.Id))!;
                requisition.Cancel(user.Id, dto.Problem);
            }

            if (dto.ApprovalStatus != null)
            {
                await HandleApprovalAsync(requisition, dto, identity);
            }

            if (dto.Items != null)
            {
                if (!identity.IsProductionUser)
                {
                    throw new UnauthorizedException("Unauthorized to change items.");
                }

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

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<PurchaseRequisitionDto>(requisition);
        }

        private async Task HandleApprovalAsync(
            PurchaseRequisition requisition,
            PurchaseRequisitionUpdateDto dto,
            Identity identity)
        {
            bool isFinance = identity.IsFinanceUser;
            bool isManager = identity.Roles.Contains("ProductionManager");

            if (!isFinance && !isManager)
            {
                throw new UnauthorizedException("Not authorized to handle approval.");
            }

            User user = (await _userManager.FindByIdAsync(identity.Id))!;

            if (dto.ApprovalStatus == ApprovalStatusOption.Approved)
            {
                if (isManager)
                {
                    requisition.ApproveAsProductionManager(user);
                }

                if (isFinance)
                {
                    requisition.ApproveAsFinance(user);
                }
            }
            else if (dto.ApprovalStatus == ApprovalStatusOption.Rejected)
            {
                if (dto.Problem == null)
                {
                    throw new InvalidDomainOperationException(
                            "Cannot reject a requisition without a problem."
                        );
                }
                requisition.Reject(user.Id, dto.Problem);
            }
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
