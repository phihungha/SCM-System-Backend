using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.Models;
using ScmssApiServer.Services;

namespace ScmssApiServer.Controllers
{
    public abstract class CustomControllerBase : ControllerBase
    {
        protected readonly UserManager<User> _userManager;

        public CustomControllerBase(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public Identity CurrentIdentity => Identity.FromClaims(User, _userManager);

        [NonAction]
        public OkObjectResult OkMessage(string message)
        {
            return Ok(new { Title = message });
        }
    }
}
