using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.IDomainServices;

namespace ScmssApiServer.Controllers
{
    public abstract class CustomControllerBase : ControllerBase
    {
        protected readonly IUsersService _usersService;

        public CustomControllerBase(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // Auth check ensures a user ID always exists
        public string CurrentUserId => _usersService.GetUserIdFromPrincipal(User)!;

        [NonAction]
        public OkObjectResult OkMessage(string message)
        {
            return Ok(new { Title = message });
        }
    }
}
