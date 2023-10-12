using Microsoft.AspNetCore.Mvc;
using SCM_System_Api_Server.DomainServices;
using SCM_System_Api_Server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SCM_System_Api_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _usersService.GetUsers();
        }

        // GET: api/<UsersController>/profileImageUploadUrl
        [HttpGet("{id}/profileImageUploadUrl")]
        public string GetProfileImageUploadUrl(long id)
        {
            return _usersService.GetUserProfileImageUploadUrl(id);
        }
    }
}