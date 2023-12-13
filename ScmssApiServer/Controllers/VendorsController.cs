using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.Controllers
{
    [Authorize(Roles = "PurchaseSpecialist,PurchaseManager")]
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : CustomControllerBase
    {
        private readonly IVendorsService _vendorsService;

        public VendorsController(IVendorsService vendorsService, UserManager<User> userManager)
            : base(userManager)
        {
            _vendorsService = vendorsService;
        }

        [Authorize(Roles = "PurchaseManager")]
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> Create([FromBody] CompanyInputDto body)
        {
            CompanyDto item = await _vendorsService.AddAsync(body);
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> Get(int id)
        {
            CompanyDto? item = await _vendorsService.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IList<CompanyDto>>> GetMany([FromQuery] SimpleQueryDto query)
        {
            IList<CompanyDto> items = await _vendorsService.GetManyAsync(query);
            return Ok(items);
        }

        [Authorize(Roles = "PurchaseManager")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<CompanyDto>> Update(int id, [FromBody] CompanyInputDto body)
        {
            CompanyDto item = await _vendorsService.UpdateAsync(id, body);
            return Ok(item);
        }
    }
}
