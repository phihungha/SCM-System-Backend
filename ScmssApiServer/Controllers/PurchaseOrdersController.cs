﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Utilities;

namespace ScmssApiServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : CustomControllerBase
    {
        private readonly IPurchaseOrdersService _purchaseOrdersService;

        public PurchaseOrdersController(IPurchaseOrdersService purchaseOrdersService,
                                        IUsersService usersService)
            : base(usersService)
        {
            _purchaseOrdersService = purchaseOrdersService;
        }

        [HttpPost("{orderId}/events")]
        public async Task<ActionResult<TransOrderEventDto>> AddManualEvent(
            int orderId,
            [FromBody] TransOrderEventCreateDto body)
        {
            TransOrderEventDto item = await _purchaseOrdersService.AddManualEventAsync(orderId, body);
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseOrderDto>> Create([FromBody] PurchaseOrderCreateDto body)
        {
            PurchaseOrderDto item = await _purchaseOrdersService.CreateAsync(body, CurrentUserId);
            return Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<IList<PurchaseOrderDto>>> Get()
        {
            IList<PurchaseOrderDto> items = await _purchaseOrdersService.GetManyAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrderDto>> GetId(int id)
        {
            PurchaseOrderDto? item = await _purchaseOrdersService.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<PurchaseOrderDto>> Update(
            int id,
            [FromBody] PurchaseOrderUpdateDto body)
        {
            PurchaseOrderDto item = await _purchaseOrdersService.UpdateAsync(id, body, CurrentUserId);
            return Ok(item);
        }

        [HttpPatch("{orderId}/events/{id}")]
        public async Task<ActionResult<TransOrderEventDto>> UpdateEvent(
            int orderId,
            int id,
            [FromBody] OrderEventUpdateDto body)
        {
            TransOrderEventDto item = await _purchaseOrdersService.UpdateEventAsync(id, orderId, body);
            return Ok(item);
        }
    }
}