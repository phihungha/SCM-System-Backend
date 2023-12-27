using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ScmssApiServer.Data;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;

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

            var revenueByMonth = await salesQuery
                .GroupBy(i => new { i.EndTime!.Value.Month, i.EndTime.Value.Year })
                .Select(i => new ReportChartPointDto<string, decimal>
                {
                    Name = $"{i.Key.Month}/{i.Key.Year}",
                    Value = i.Sum(i => i.TotalAmount)
                }).ToListAsync();

            var totalRevenue = salesQuery.Sum(i => i.TotalAmount);
            var averageRevenue = salesQuery.Average(i => i.TotalAmount);

            return new SalesReportDto
            {
                RevenueByMonth = revenueByMonth,
                TotalRevenue = totalRevenue,
                AverageRevenue = averageRevenue,
            };
        }
    }
}
