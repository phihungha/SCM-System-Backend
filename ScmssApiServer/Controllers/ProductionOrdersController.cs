using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;
using ScmssApiServer.Utilities;

namespace ScmssApiServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionOrdersController : CustomControllerBase
    {
        private readonly IProductionOrdersService _productionOrdersService;

        public ProductionOrdersController(IProductionOrdersService ProductionOrdersService,
                                          IUsersService usersService)
            : base(usersService)
        {
            _productionOrdersService = ProductionOrdersService;
        }

        [HttpPost("{orderId}/events")]
        public async Task<ActionResult<ProductionOrderEventDto>> AddManualEvent(
            int orderId,
            [FromBody] ProductionOrderEventCreateDto body)
        {
            ProductionOrderEventDto item = await _productionOrdersService.AddManualEventAsync(orderId, body);
            return Ok(item);
        }

        [HttpPatch("{orderId}/events/{id}")]
        public async Task<ActionResult<ProductionOrderEventDto>> AddManualEvent(
            int orderId,
            int id,
            [FromBody] OrderEventUpdateDto body)
        {
            ProductionOrderEventDto item = await _productionOrdersService.UpdateEventAsync(id, orderId, body);
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ProductionOrderDto>> Create([FromBody] ProductionOrderCreateDto body)
        {
            ProductionOrderDto item = await _productionOrdersService.CreateAsync(body, CurrentUserId);
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IList<ProductionOrderDto>>> Get()
        {
            IList<ProductionOrderDto> items = await _productionOrdersService.GetManyAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionOrderDto>> GetId(int id)
        {
            ProductionOrderDto? item = await _productionOrdersService.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ProductionOrderDto>> Update(
            int id,
            [FromBody] ProductionOrderUpdateDto body)
        {
            ProductionOrderDto item = await _productionOrdersService.UpdateAsync(id, body, CurrentUserId);
            return Ok(item);
        }
    }
}
