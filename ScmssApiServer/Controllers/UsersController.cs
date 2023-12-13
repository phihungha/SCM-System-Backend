using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.Exceptions;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : CustomControllerBase
    {
        private IUsersService _usersService;

        public UsersController(IUsersService usersService, UserManager<User> userManager)
            : base(userManager)
        {
            _usersService = usersService;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] UserCreateDto body)
        {
            try
            {
                UserDto item = await _usersService.CreateUserAsync(body);
                return Ok(item);
            }
            catch (IdentityException ex)
            {
                ex.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IList<UserDto>>> Get()
        {
            IList<UserDto> items = await _usersService.GetUsersAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetId(string id)
        {
            UserDto? item = await _usersService.GetUserAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet("{id}/profileImageUploadUrl")]
        public string GetProfileImageUploadUrl()
        {
            return _usersService.GetProfileImageUploadUrl(CurrentIdentity);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<UserDto>> Update(string id, [FromBody] UserInputDto body)
        {
            try
            {
                UserDto item = await _usersService.UpdateUserAsync(id, body);
                return Ok(item);
            }
            catch (IdentityException ex)
            {
                ex.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}/changePassword")]
        public async Task<ActionResult<UserDto>> Update(string id, [FromBody] UserPasswordChangeDto body)
        {
            try
            {
                await _usersService.ChangePasswordAsync(id, body);
                return Ok();
            }
            catch (IdentityException ex)
            {
                ex.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }
        }
    }
}
