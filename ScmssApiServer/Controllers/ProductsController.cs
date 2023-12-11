using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Utilities;

namespace ScmssApiServer.Controllers
{
    [Authorize]
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
