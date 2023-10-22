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
        public async Task<ActionResult<IList<UserGetDto>>> Get()
        {
            IList<User> users = await _usersService.GetUsersAsync();
            IList<UserGetDto> userDtos = users.Select(ToUserGetDto).ToList();
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserGetDto>> GetId(string id)
        {
            User? user = await _usersService.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserGetDto userDto = ToUserGetDto(user);
            return Ok(userDto);
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

        private UserGetDto ToUserGetDto(User user)
        {
            return new UserGetDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                IdCardNumber = user.IdCardNumber,
                Address = user.Address,
                Description = user.Description,
                IsActive = user.IsActive,
                CreatedTime = user.CreatedTime,
                UpdatedTime = user.UpdatedTime
            };
        }
    }
}
