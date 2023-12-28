using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class HomeService : IHomeService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public HomeService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<HomeDto> GetHome()
        {
            IList<PurchaseOrder> activePurchaseOrders = await _dbContext.PurchaseOrders
                .Where(i => i.EndTime != null)
                .ToListAsync();

            IList<ProductionOrder> activeProductionOrders = await _dbContext.ProductionOrders
                .Where(i => i.EndTime != null)
                .ToListAsync();

            IList<SalesOrder> activeSalesOrders = await _dbContext.SalesOrders
                .Where(i => i.EndTime != null)
                .ToListAsync();

            return new HomeDto
            {
                ActivePurchaseOrders = _mapper.Map<IList<PurchaseOrderDto>>(activePurchaseOrders),
                ActivePurchaseOrderCount = activePurchaseOrders.Count(),
                ActiveProductionOrders = _mapper.Map<IList<ProductionOrderDto>>(activeProductionOrders),
                ActiveProductionOrderCount = activeProductionOrders.Count(),
                ActiveSalesOrders = _mapper.Map<IList<SalesOrderDto>>(activeSalesOrders),
                ActiveSalesOrderCount = activeSalesOrders.Count(),
            };
        }
    }
}
