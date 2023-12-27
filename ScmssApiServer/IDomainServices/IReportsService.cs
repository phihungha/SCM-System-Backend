using ScmssApiServer.DTOs;

namespace ScmssApiServer.IDomainServices
{
    public interface IReportsService
    {
        Task<SalesReportDto> GetSales(ReportQueryDto dto);
    }
}
