using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.DTOs;
using ScmssApiServer.Exceptions;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;
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

        [HttpGet]
        public async Task<ActionResult<IList<ProductDto>>> Get()
        {
            IList<ProductDto> items = await _productsService.GetManyAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetId(int id)
        {
            ProductDto? item = await _productsService.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] ProductInputDto body)
        {
            ProductDto item = await _productsService.CreateAsync(body);
            return Ok(item);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDto>> Delete(int id)
        {
            try
            {
                await _productsService.DeleteAsync(id);
                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
