using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.Controllers
{
    [Authorize(Roles = "InventorySpecialist,InventoryManager")]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : CustomControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService, UserManager<User> userManager)
            : base(userManager)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("{facilityId}/Products/{id}")]
        public async Task<ActionResult<WarehouseProductItemDto>> GetProduct(int facilityId, int id)
        {
            WarehouseProductItemDto? dto = await _inventoryService.GetProduct(facilityId, id, CurrentIdentity);
            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }

        [HttpGet("{facilityId}/ProductionOrdersToReceive")]
        public async Task<ActionResult<IList<ProductionOrderDto>>> GetProductionOrdersForIssue(
            int facilityId, [FromQuery] InventoryOrderQueryDto query)
        {
            IList<ProductionOrderDto> dtos = await _inventoryService.GetProductionOrdersToReceive(facilityId, query, CurrentIdentity);
            return Ok(dtos);
        }

        [HttpGet("{facilityId}/ProductionOrdersToIssue")]
        public async Task<ActionResult<IList<ProductionOrderDto>>> GetProductionOrdersToIssue(
            int facilityId, [FromQuery] InventoryOrderQueryDto query)
        {
            IList<ProductionOrderDto> dtos = await _inventoryService.GetProductionOrdersToIssue(facilityId, query, CurrentIdentity);
            return Ok(dtos);
        }

        [HttpGet("{facilityId}/Products")]
        public async Task<ActionResult<IList<WarehouseProductItemDto>>> GetProducts(
            int facilityId, [FromQuery] SimpleQueryDto query)
        {
            IList<WarehouseProductItemDto> dtos = await _inventoryService.GetProducts(facilityId, query, CurrentIdentity);
            return Ok(dtos);
        }

        [HttpGet("{facilityId}/PurchaseOrdersToReceive")]
        public async Task<ActionResult<IList<PurchaseOrderDto>>> GetPurchaseOrdersToReceive(
            int facilityId, [FromQuery] InventoryOrderQueryDto query)
        {
            IList<PurchaseOrderDto> dtos = await _inventoryService.GetPurchaseOrdersToReceive(facilityId, query, CurrentIdentity);
            return Ok(dtos);
        }

        [HttpGet("{facilityId}/SalesOrdersToIssue")]
        public async Task<ActionResult<IList<SalesOrderDto>>> GetSalesOrdersToIssue(
            int facilityId, [FromQuery] InventoryOrderQueryDto query)
        {
            IList<SalesOrderDto> dtos = await _inventoryService.GetSalesOrdersToIssue(facilityId, query, CurrentIdentity);
            return Ok(dtos);
        }

        [HttpGet("{facilityId}/Supplies")]
        public async Task<ActionResult<IList<WarehouseSupplyItemDto>>> GetSupplies(
            int facilityId, [FromQuery] SimpleQueryDto query)
        {
            IList<WarehouseSupplyItemDto> dtos = await _inventoryService.GetSupplies(facilityId, query, CurrentIdentity);
            return Ok(dtos);
        }

        [HttpGet("{facilityId}/Supplies/{id}")]
        public async Task<ActionResult<WarehouseSupplyItemDto>> GetSupply(int facilityId, int id)
        {
            WarehouseSupplyItemDto? dto = await _inventoryService.GetSupply(facilityId, id, CurrentIdentity);
            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }

        [HttpPatch("{facilityId}/Products")]
        public async Task<ActionResult<IList<WarehouseProductItemDto>>> UpdateProducts(
            int facilityId, [FromBody] WarehouseUpdateDto body)
        {
            IList<WarehouseProductItemDto> items = await _inventoryService.UpdateProducts(facilityId, body, CurrentIdentity);
            return Ok(items);
        }

        [HttpPatch("{facilityId}/Supplies")]
        public async Task<ActionResult<IList<WarehouseSupplyItemDto>>> UpdateSupplies(
            int facilityId, [FromBody] WarehouseUpdateDto body)
        {
            IList<WarehouseSupplyItemDto> items = await _inventoryService.UpdateSupplies(
                facilityId, body, CurrentIdentity);
            return Ok(items);
        }
    }
}
