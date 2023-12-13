using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.Controllers
{
    [Authorize(Roles = "PurchaseSpecialist,PurchaseManager")]
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliesController : CustomControllerBase
    {
        private ISuppliesService _suppliesService;

        public SuppliesController(ISuppliesService suppliesService, UserManager<User> userManager)
            : base(userManager)
        {
            _suppliesService = suppliesService;
        }

        [Authorize(Roles = "PurchaseManager")]
        [HttpPost]
        public async Task<ActionResult<SupplyDto>> Create([FromBody] SupplyInputDto body)
        {
            SupplyDto item = await _suppliesService.CreateAsync(body);
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplyDto>> Get(int id)
        {
            SupplyDto? item = await _suppliesService.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IList<ProductDto>>> GetMany([FromQuery] SimpleQueryDto query)
        {
            IList<SupplyDto> items = await _suppliesService.GetManyAsync(query);
            return Ok(items);
        }

        [Authorize(Roles = "PurchaseManager")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] SupplyInputDto body)
        {
            try
            {
                SupplyDto item = await _suppliesService.UpdateAsync(id, body);
                return Ok(item);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
