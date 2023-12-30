namespace ScmssApiServer.DTOs
{
    public class PurchaseReportDto
    {
        public decimal AverageCost { get; set; }
        public double AverageDeliveryTime { get; set; }
        public required IList<ReportChartPointDto<string, double>> AverageDeliveryTimeByMonth { get; set; }
        public required IList<ReportChartPointDto<string, decimal>> CostByMonth { get; set; }
        public required IList<ReportListItemDto<PurchaseOrderDto, decimal>> HighestCostOrders { get; set; }
        public required IList<ReportListItemDto<SupplyDto, double>> MostBoughtSupplies { get; set; }
        public required IList<ReportListItemDto<CompanyDto, int>> MostFrequentVendors { get; set; }
        public required IList<ReportChartPointDto<string, int>> OrderCountByFinalStatus { get; set; }
        public decimal TotalCost { get; set; }
    }
}
