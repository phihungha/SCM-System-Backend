using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IReportsService
    {
        Task<SalesReportDto> GetSales(ReportQueryDto dto);
        Task<ProductionReportDto> GetProduction(ReportQueryDto dto);
    }
}
