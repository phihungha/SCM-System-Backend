using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Utilities;

namespace ScmssApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalesOrdersController : CustomControllerBase
    {
        private readonly ISalesOrdersService _salesOrdersService;

        public SalesOrdersController(ISalesOrdersService salesOrdersService,
                                     IUsersService usersService)
            : base(usersService)
        {
            _salesOrdersService = salesOrdersService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<SalesOrderDto>>> Get()
        {
            IList<SalesOrderDto> items = await _salesOrdersService.GetSalesOrdersAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrderDto>> GetId(int id)
        {
            SalesOrderDto? item = await _salesOrdersService.GetSalesOrderAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<SalesOrderDto>> Create([FromBody] SalesOrderCreateDto body)
        {
            SalesOrderDto item = await _salesOrdersService.CreateSalesOrderAsync(body, CurrentUserId);
            return Ok(item);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<SalesOrderDto>> Update(int id, [FromBody] SalesOrderInputDto body)
        {
            SalesOrderDto item = await _salesOrdersService.UpdateSalesOrderAsync(id, body, CurrentUserId);
            return Ok(item);
        }
    }
}
