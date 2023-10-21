using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<User>>> Get()
        {
            return Ok(await _usersService.GetUsersAsync());
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] UserCreateDto body)
        {
            try
            {
                User newData = await _usersService.CreateUserAsync(body);
                return Ok(newData);
            }
            catch (IdentityException ex)
            {
                ex.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }
        }

        [HttpGet("{id}/profileImageUploadUrl")]
        public string GetProfileImageUploadUrl(string id)
        {
            return _usersService.GetProfileImageUploadUrl(id);
        }
    }
}
