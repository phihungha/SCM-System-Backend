namespace ScmssApiServer.DTOs
{
    public class ProductionReportDto
    {
        public decimal AverageCost { get; set; }
        public double AverageProductionTime { get; set; }
        public required IList<ReportChartPointDto<string, double>> AverageProductionTimeByMonth { get; set; }
        public decimal AverageValue { get; set; }
        public required IList<ReportChartPointDto<string, decimal>> CostByMonth { get; set; }
        public required IList<ReportListItemDto<ProductionOrderDto, decimal>> HighestValueOrders { get; set; }
        public required IList<ReportListItemDto<ProductionFacilityDto, int>> MostFrequentFacilities { get; set; }
        public required IList<ReportListItemDto<ProductDto, double>> MostProducedProducts { get; set; }
        public required IList<ReportListItemDto<SupplyDto, double>> MostUsedSupplies { get; set; }
        public required IList<ReportChartPointDto<string, int>> OrderCountByFinalStatus { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalValue { get; set; }
        public required IList<ReportChartPointDto<string, decimal>> ValueByMonth { get; set; }
    }
}
