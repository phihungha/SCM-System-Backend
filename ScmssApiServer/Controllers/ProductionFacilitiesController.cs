using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.Controllers
{
    [Authorize(Roles = "Director,ProductionPlanner,ProductionManager")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionFacilitiesController : CustomControllerBase
    {
        private readonly IProductionFacilitiesService _facilitiesService;

        public ProductionFacilitiesController(IProductionFacilitiesService facilitiesService,
                                              UserManager<User> userManager)
            : base(userManager)
        {
            _facilitiesService = facilitiesService;
        }

        [Authorize(Roles = "ProductionManager")]
        [HttpPost]
        public async Task<ActionResult<ProductionFacilityDto>> Create([FromBody] ProductionFacilityInputDto body)
        {
            ProductionFacilityDto item = await _facilitiesService.AddAsync(body);
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionFacilityDto>> Get(int id)
        {
            ProductionFacilityDto? item = await _facilitiesService.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IList<ProductionFacilityDto>>> GetMany([FromQuery] SimpleQueryDto query)
        {
            IList<ProductionFacilityDto> items = await _facilitiesService.GetManyAsync(query);
            return Ok(items);
        }

        [Authorize(Roles = "ProductionManager")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<ProductionFacilityDto>> Update(
            int id,
            [FromBody] ProductionFacilityInputDto body)
        {
            ProductionFacilityDto item = await _facilitiesService.UpdateAsync(id, body);
            return Ok(item);
        }
    }
}
