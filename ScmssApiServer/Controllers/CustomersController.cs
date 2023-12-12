using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Utilities;

namespace ScmssApiServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : CustomControllerBase
    {
        private readonly ICustomersService _customersService;

        public CustomersController(ICustomersService customersService,
                                 IUsersService usersService)
            : base(usersService)
        {
            _customersService = customersService;
        }

        [HttpPost]
        public async Task<ActionResult<CompanyDto>> Create([FromBody] CompanyInputDto body)
        {
            CompanyDto item = await _customersService.AddAsync(body);
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> Get(int id)
        {
            CompanyDto? item = await _customersService.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IList<CompanyDto>>> GetMany([FromQuery] SimpleQueryDto query)
        {
            IList<CompanyDto> items = await _customersService.GetManyAsync(query);
            return Ok(items);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<CompanyDto>> Update(int id, [FromBody] CompanyInputDto body)
        {
            CompanyDto item = await _customersService.UpdateAsync(id, body);
            return Ok(item);
        }
    }
}
