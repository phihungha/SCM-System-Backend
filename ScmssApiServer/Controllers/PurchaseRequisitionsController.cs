using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;

namespace ScmssApiServer.Controllers
{
    [Authorize(Roles =
        "Admin," +
        "ProductionPlanner," +
        "ProductionManager," +
        "Finance," +
        "PurchaseSpecialist," +
        "PurchaseManager")]
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

        [Authorize(Roles = "Admin,ProductionPlanner,ProductionManager")]
        [HttpPost]
        public async Task<ActionResult<PurchaseRequisitionDto>> Create([FromBody] PurchaseRequisitionCreateDto body)
        {
            PurchaseRequisitionDto item = await _purchaseRequisitionsService.CreateAsync(body, CurrentUserId);
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseRequisitionDto>> Get(int id)
        {
            PurchaseRequisitionDto? item = await _purchaseRequisitionsService.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IList<PurchaseRequisitionDto>>> GetMany()
        {
            IList<PurchaseRequisitionDto> items = await _purchaseRequisitionsService.GetManyAsync();
            return Ok(items);
        }
        [Authorize(Roles = "Admin,ProductionPlanner,ProductionManager,Finance")]
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
