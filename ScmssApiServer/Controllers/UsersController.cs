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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : CustomControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<UserDto>>> Get()
        {
            IList<UserDto> result = await _usersService.GetUsersAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetId(string id)
        {
            UserDto? result = await _usersService.GetUserAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] UserCreateDto body)
        {
            try
            {
                UserDto result = await _usersService.CreateUserAsync(body);
                return Ok(result);
            }
            catch (IdentityException ex)
            {
                ex.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<User>> Update(string id, [FromBody] UserUpdateDto body)
        {
            try
            {
                UserDto result = await _usersService.UpdateUserAsync(id, body);
                return Ok(result);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (IdentityException ex)
            {
                ex.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(string id)
        {
            try
            {
                await _usersService.DeleteUserAsync(id);
                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/profileImageUploadUrl")]
        public string GetProfileImageUploadUrl(string id)
        {
            return _usersService.GetProfileImageUploadUrl(id);
        }
    }
}
