using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IReportsService
    {
        Task<ProductionReportDto> GetProduction(ReportQueryDto dto);

        Task<PurchaseReportDto> GetPurchase(ReportQueryDto dto);

        Task<SalesReportDto> GetSales(ReportQueryDto dto);
    }
}
