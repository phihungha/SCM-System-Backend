using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;

namespace ScmssApiServer.Controllers
{
    [Authorize(Roles = "Admin,ProductionPlanner,ProductionManager,SalesSpecialist,SalesManager")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : CustomControllerBase
    {
        private IProductsService _productsService;

        public ProductsController(IProductsService productsService, IUsersService usersService)
            : base(usersService)
        {
            _productsService = productsService;
        }

        [Authorize(Roles = "Admin,ProductionManager,SalesManager")]
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] ProductInputDto body)
        {
            ProductDto item = await _productsService.CreateAsync(body);
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            ProductDto? item = await _productsService.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IList<ProductDto>>> GetMany([FromQuery] SimpleQueryDto query)
        {
            IList<ProductDto> items = await _productsService.GetManyAsync(query);
            return Ok(items);
        }

        [Authorize(Roles = "Admin,ProductionManager,SalesManager")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] ProductInputDto body)
        {
            try
            {
                ProductDto item = await _productsService.UpdateAsync(id, body);
                return Ok(item);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
