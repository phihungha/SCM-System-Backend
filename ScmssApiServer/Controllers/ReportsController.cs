using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : CustomControllerBase
    {
        private readonly IReportsService _reportsService;

        public ReportsController(IReportsService reportsService, UserManager<User> userManager)
            : base(userManager)
        {
            _reportsService = reportsService;
        }

        [HttpGet("production")]
        public async Task<ActionResult<ProductionReportDto>> GetProduction([FromQuery] ReportQueryDto query)
        {
            ProductionReportDto item = await _reportsService.GetProduction(query);
            return Ok(item);
        }

        [HttpGet("purchase")]
        public async Task<ActionResult<PurchaseReportDto>> GetPurchase([FromQuery] ReportQueryDto query)
        {
            PurchaseReportDto item = await _reportsService.GetPurchase(query);
            return Ok(item);
        }

        [HttpGet("sales")]
        public async Task<ActionResult<SalesReportDto>> GetSales([FromQuery] ReportQueryDto query)
        {
            SalesReportDto item = await _reportsService.GetSales(query);
            return Ok(item);
        }
    }
}
