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

        [HttpGet("{facilityId}/Products")]
        public async Task<ActionResult<IList<WarehouseProductItemDto>>> GetProducts(int facilityId, [FromQuery] SimpleQueryDto query)
        {
            IList<WarehouseProductItemDto>? dtos
                = await _inventoryService.GetProducts(facilityId, query, CurrentIdentity);
            if (dtos == null)
            {
                return NotFound();
            }
            return Ok(dtos);
        }

        [HttpGet("{facilityId}/Supplies")]
        public async Task<ActionResult<IList<WarehouseSupplyItemDto>>> GetSupplies(int facilityId, [FromQuery] SimpleQueryDto query)
        {
            IList<WarehouseSupplyItemDto>? dtos
                = await _inventoryService.GetSupplies(facilityId, query, CurrentIdentity);
            if (dtos == null)
            {
                return NotFound();
            }
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
    }
}
