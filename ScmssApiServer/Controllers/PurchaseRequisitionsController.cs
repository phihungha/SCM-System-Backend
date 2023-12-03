using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Utilities;

namespace ScmssApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseRequisitionsController : CustomControllerBase
    {
        private readonly IPurchaseRequisitionsService _purchaseRequisitionsService;

        public PurchaseRequisitionsController(IPurchaseRequisitionsService purchaseRequisitionsService,
                                              IUsersService usersService)
            : base(usersService)
        {
            _purchaseRequisitionsService = purchaseRequisitionsService;
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseRequisitionDto>> Create([FromBody] PurchaseRequisitionCreateDto body)
        {
            PurchaseRequisitionDto item = await _purchaseRequisitionsService.CreateAsync(body, CurrentUserId);
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IList<PurchaseRequisitionDto>>> Get()
        {
            IList<PurchaseRequisitionDto> items = await _purchaseRequisitionsService.GetManyAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseRequisitionDto>> GetId(int id)
        {
            PurchaseRequisitionDto? item = await _purchaseRequisitionsService.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<PurchaseRequisitionDto>> Update(
            int id,
            [FromBody] PurchaseRequisitionUpdateDto body)
        {
            PurchaseRequisitionDto item = await _purchaseRequisitionsService.UpdateAsync(id, body, CurrentUserId);
            return Ok(item);
        }
    }
}
