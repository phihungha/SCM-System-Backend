using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;

namespace ScmssApiServer.Controllers
{
    [Authorize(Roles = "Director,ProductionPlanner,ProductionManager")]
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

        [Authorize(Roles = "ProductionPlanner,ProductionManager")]
        [HttpPost("{orderId}/events")]
        public async Task<ActionResult<ProductionOrderEventDto>> AddManualEvent(
            int orderId,
            [FromBody] ProductionOrderEventCreateDto body)
        {
            ProductionOrderEventDto item = await _productionOrdersService.AddManualEventAsync(orderId, body);
            return Ok(item);
        }

        [Authorize(Roles = "ProductionPlanner,ProductionManager")]
        [HttpPost]
        public async Task<ActionResult<ProductionOrderDto>> Create([FromBody] ProductionOrderCreateDto body)
        {
            ProductionOrderDto item = await _productionOrdersService.CreateAsync(body, CurrentIdentity);
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionOrderDto>> Get(int id)
        {
            ProductionOrderDto? item = await _productionOrdersService.GetAsync(id, CurrentIdentity);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IList<ProductionOrderDto>>> GetMany()
        {
            IList<ProductionOrderDto> items = await _productionOrdersService.GetManyAsync(CurrentIdentity);
            return Ok(items);
        }

        [Authorize(Roles = "ProductionPlanner,ProductionManager")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<ProductionOrderDto>> Update(
            int id,
            [FromBody] ProductionOrderUpdateDto body)
        {
            ProductionOrderDto item = await _productionOrdersService.UpdateAsync(id, body, CurrentIdentity);
            return Ok(item);
        }

        [Authorize(Roles = "ProductionPlanner,ProductionManager")]
        [HttpPatch("{orderId}/events/{id}")]
        public async Task<ActionResult<ProductionOrderEventDto>> UpdateEvent(
            int orderId,
            int id,
            [FromBody] OrderEventUpdateDto body)
        {
            ProductionOrderEventDto item = await _productionOrdersService.UpdateEventAsync(id, orderId, body);
            return Ok(item);
        }
    }
}
