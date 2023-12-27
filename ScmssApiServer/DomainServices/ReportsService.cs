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

        public async Task<SalesReportDto> GetSales(ReportQueryDto dto)
        {
            var startTime = new DateTime(dto.StartYear, dto.StartMonth, 1).ToUniversalTime();
            var endTime = new DateTime(
                dto.EndYear,
                dto.EndMonth,
                DateTime.DaysInMonth(dto.EndYear, dto.EndMonth)).ToUniversalTime();

            var salesQuery = _dbContext.SalesOrders
                .Where(i => i.EndTime != null &&
                       i.EndTime.Value >= startTime &&
                       i.EndTime.Value <= endTime);

            var paidSalesQuery = salesQuery.Where(
                i => i.PaymentStatus == TransOrderPaymentStatus.Completed);

            var revenueByMonth = await paidSalesQuery
                .GroupBy(i => new { i.EndTime!.Value.Month, i.EndTime.Value.Year })
                .Select(i => new ReportChartPointDto<string, decimal>
                {
                    Name = $"{i.Key.Month}/{i.Key.Year}",
                    Value = i.Sum(j => j.TotalAmount)
                }).ToListAsync();
            decimal totalRevenue = await paidSalesQuery.SumAsync(i => i.TotalAmount);
            decimal averageRevenue = await paidSalesQuery.AverageAsync(i => (decimal?)i.TotalAmount) ?? 0;

            var completedSalesQuery = salesQuery.Where(i => i.Status == OrderStatus.Completed);

            var averageDeliveryTimeByMonth = await completedSalesQuery
                .GroupBy(i => new { i.ExecutionFinishTime!.Value.Month, i.ExecutionFinishTime.Value.Year })
                .Select(i => new ReportChartPointDto<string, double>
                {
                    Name = $"{i.Key.Month}/{i.Key.Year}",
                    Value = i.Average(j => j.ExecutionDuration!.Value.TotalDays),
                }).ToListAsync();
            var averageDeliveryTime = await completedSalesQuery
                .AverageAsync(i => (double?)i.ExecutionDuration!.Value.TotalDays) ?? 0;

            var highestValueOrders = await completedSalesQuery
                .OrderByDescending(i => i.TotalAmount)
                .Select(i => new ReportListItemDto<SalesOrderDto, decimal>
                {
                    Item = _mapper.Map<SalesOrderDto>(i),
                    Value = i.TotalAmount,
                }).ToListAsync();

            var mostFrequentCustomers = await completedSalesQuery
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

            var orderCountByFinalStatus = await salesQuery
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
