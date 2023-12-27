namespace ScmssApiServer.DTOs
{
    public class SalesReportDto
    {
        public double AverageDeliveryTime { get; set; }
        public required IList<ReportChartPointDto<string, double>> AverageDeliveryTimeByMonth { get; set; }
        public decimal AverageRevenue { get; set; }
        public required IList<ReportListItemDto<SalesOrderDto, decimal>> HighestValueOrders { get; set; }
        public required IList<ReportListItemDto<CompanyDto, int>> MostFrequentCustomers { get; set; }
        public required IList<ReportListItemDto<ProductDto, double>> MostPopularProducts { get; set; }
        public required IList<ReportChartPointDto<string, int>> OrderCountByFinalStatus { get; set; }
        public required IList<ReportChartPointDto<string, decimal>> RevenueByMonth { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
