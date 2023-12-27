using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.DomainServices
{
    public class ReportsService : IReportsService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ReportsService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProductionReportDto> GetProduction(ReportQueryDto dto)
        {
            var startTime = new DateTime(dto.StartYear, dto.StartMonth, 1).ToUniversalTime();
            var endTime = new DateTime(
                dto.EndYear,
                dto.EndMonth,
                DateTime.DaysInMonth(dto.EndYear, dto.EndMonth)).ToUniversalTime();

            var orderQuery = _dbContext.ProductionOrders
                .Where(i => i.EndTime != null &&
                       i.EndTime.Value >= startTime &&
                       i.EndTime.Value <= endTime);

            var completedOrderQuery = orderQuery.Where(i => i.Status == OrderStatus.Completed);

            var valueByMonth = await completedOrderQuery
                .GroupBy(i => new { i.EndTime!.Value.Month, i.EndTime.Value.Year })
                .Select(i => new ReportChartPointDto<string, decimal>
                {
                    Name = $"{i.Key.Year}-{i.Key.Month}",
                    Value = i.Sum(j => j.TotalValue)
                }).ToListAsync();
            decimal totalValue = await completedOrderQuery.SumAsync(i => i.TotalValue);
            decimal averageValue = await completedOrderQuery.AverageAsync(i => (decimal?)i.TotalValue) ?? 0;

            var costByMonth = await completedOrderQuery
                .GroupBy(i => new { i.EndTime!.Value.Month, i.EndTime.Value.Year })
                .Select(i => new ReportChartPointDto<string, decimal>
                {
                    Name = $"{i.Key.Year}-{i.Key.Month}",
                    Value = i.Sum(j => j.TotalCost)
                }).ToListAsync();
            decimal totalCost = await completedOrderQuery.SumAsync(i => i.TotalCost);
            decimal averageCost = await completedOrderQuery.AverageAsync(i => (decimal?)i.TotalCost) ?? 0;

            var averayeProductionTimeByMonth = await completedOrderQuery
                .GroupBy(i => new
                {
                    i.ExecutionFinishTime!.Value.Month,
                    i.ExecutionFinishTime.Value.Year
                })
                .Select(i => new ReportChartPointDto<string, double>
                {
                    Name = $"{i.Key.Year}-{i.Key.Month}",
                    Value = i.Average(j => j.ExecutionDuration!.Value.TotalDays),
                }).ToListAsync();
            var averageProductionTime = await completedOrderQuery
                .AverageAsync(i => (double?)i.ExecutionDuration!.Value.TotalDays) ?? 0;

            var highestValueOrders = await completedOrderQuery
                .OrderByDescending(i => i.TotalValue)
                .Select(i => new ReportListItemDto<ProductionOrderDto, decimal>
                {
                    Item = _mapper.Map<ProductionOrderDto>(i),
                    Value = i.TotalValue,
                }).ToListAsync();

            var mostFrequentFacilities = await completedOrderQuery
                .Include(i => i.ProductionFacility)
                .GroupBy(i => i.ProductionFacility)
                .Select(i => new ReportListItemDto<ProductionFacilityDto, int>
                {
                    Item = _mapper.Map<ProductionFacilityDto>(i.Key),
                    Value = i.Count(),
                }).ToListAsync();

            var mostProducedProducts = await _dbContext.ProductionOrderItems
                .Include(i => i.ProductionOrder)
                .Include(i => i.Product)
                .Where(i => i.ProductionOrder.EndTime != null &&
                       i.ProductionOrder.EndTime.Value >= startTime &&
                       i.ProductionOrder.EndTime.Value <= endTime)
                .GroupBy(i => i.Product)
                .Select(i => new ReportListItemDto<ProductDto, double>
                {
                    Item = _mapper.Map<ProductDto>(i.Key),
                    Value = i.Sum(j => j.Quantity)
                })
                .OrderByDescending(i => i.Value)
                .ToListAsync();

            var mostUsedSupplies = await _dbContext.ProductionOrderSupplyUsageItems
                .Include(i => i.ProductionOrder)
                .Include(i => i.Supply)
                .Where(i => i.ProductionOrder.EndTime != null &&
                       i.ProductionOrder.EndTime.Value >= startTime &&
                       i.ProductionOrder.EndTime.Value <= endTime)
                .GroupBy(i => i.Supply)
                .Select(i => new ReportListItemDto<SupplyDto, double>
                {
                    Item = _mapper.Map<SupplyDto>(i.Key),
                    Value = i.Sum(j => j.Quantity)
                })
                .OrderByDescending(i => i.Value)
                .ToListAsync();

            var orderCountByFinalStatus = await orderQuery
            .GroupBy(i => i.Status)
            .Select(i => new ReportChartPointDto<string, int>
            {
                Name = i.Key.ToString(),
                Value = i.Count()
            }).ToListAsync();

            return new ProductionReportDto
            {
                AverageCost = averageCost,
                AverageValue = averageValue,
                ValueByMonth = valueByMonth,
                TotalValue = totalValue,
                CostByMonth = costByMonth,
                TotalCost = totalCost,
                AverageProductionTimeByMonth = averayeProductionTimeByMonth,
                AverageProductionTime = averageProductionTime,
                OrderCountByFinalStatus = orderCountByFinalStatus,
                HighestValueOrders = highestValueOrders,
                MostProducedProducts = mostProducedProducts,
                MostFrequentFacilities = mostFrequentFacilities,
                MostUsedSupplies = mostUsedSupplies,
            };
        }

        public async Task<SalesReportDto> GetSales(ReportQueryDto dto)
        {
            var startTime = new DateTime(dto.StartYear, dto.StartMonth, 1).ToUniversalTime();
            var endTime = new DateTime(
                dto.EndYear,
                dto.EndMonth,
                DateTime.DaysInMonth(dto.EndYear, dto.EndMonth)).ToUniversalTime();

            var orderQuery = _dbContext.SalesOrders
                .Where(i => i.EndTime != null &&
                       i.EndTime.Value >= startTime &&
                       i.EndTime.Value <= endTime);

            var paidOrderQuery = orderQuery.Where(
                i => i.PaymentStatus == TransOrderPaymentStatus.Completed);

            var revenueByMonth = await paidOrderQuery
                .GroupBy(i => new { i.EndTime!.Value.Month, i.EndTime.Value.Year })
                .Select(i => new ReportChartPointDto<string, decimal>
                {
                    Name = $"{i.Key.Year}-{i.Key.Month}",
                    Value = i.Sum(j => j.TotalAmount)
                }).ToListAsync();
            decimal totalRevenue = await paidOrderQuery.SumAsync(i => i.TotalAmount);
            decimal averageRevenue = await paidOrderQuery.AverageAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var completedOrderQuery = orderQuery.Where(i => i.Status == OrderStatus.Completed);

            var averageDeliveryTimeByMonth = await completedOrderQuery
                .GroupBy(i => new
                {
                    i.ExecutionFinishTime!.Value.Month,
                    i.ExecutionFinishTime.Value.Year
                })
                .Select(i => new ReportChartPointDto<string, double>
                {
                    Name = $"{i.Key.Year}-{i.Key.Month}",
                    Value = i.Average(j => j.ExecutionDuration!.Value.TotalDays),
                }).ToListAsync();
            var averageDeliveryTime = await completedOrderQuery
                .AverageAsync(i => (double?)i.ExecutionDuration!.Value.TotalDays) ?? 0;

            var highestValueOrders = await completedOrderQuery
                .OrderByDescending(i => i.TotalAmount)
                .Select(i => new ReportListItemDto<SalesOrderDto, decimal>
                {
                    Item = _mapper.Map<SalesOrderDto>(i),
                    Value = i.TotalAmount,
                }).ToListAsync();

            var mostFrequentCustomers = await completedOrderQuery
                .Include(i => i.Customer)
                .GroupBy(i => i.Customer)
                .Select(i => new ReportListItemDto<CompanyDto, int>
                {
                    Item = _mapper.Map<CompanyDto>(i.Key),
                    Value = i.Count(),
                }).ToListAsync();

            var mostPopularProducts = await _dbContext.SalesOrderItems
                .Include(i => i.SalesOrder)
                .Include(i => i.Product)
                .Where(i => i.SalesOrder.EndTime != null &&
                       i.SalesOrder.EndTime.Value >= startTime &&
                       i.SalesOrder.EndTime.Value <= endTime)
                .GroupBy(i => i.Product)
                .Select(i => new ReportListItemDto<ProductDto, double>
                {
                    Item = _mapper.Map<ProductDto>(i.Key),
                    Value = i.Sum(j => j.Quantity)
                })
                .OrderByDescending(i => i.Value)
                .ToListAsync();

            var orderCountByFinalStatus = await orderQuery
            .GroupBy(i => i.Status)
            .Select(i => new ReportChartPointDto<string, int>
            {
                Name = i.Key.ToString(),
                Value = i.Count()
            }).ToListAsync();

            return new SalesReportDto
            {
                RevenueByMonth = revenueByMonth,
                TotalRevenue = totalRevenue,
                AverageRevenue = averageRevenue,
                AverageDeliveryTimeByMonth = averageDeliveryTimeByMonth,
                AverageDeliveryTime = averageDeliveryTime,
                OrderCountByFinalStatus = orderCountByFinalStatus,
                HighestValueOrders = highestValueOrders,
                MostPopularProducts = mostPopularProducts,
                MostFrequentCustomers = mostFrequentCustomers,
            };
        }
    }
}
