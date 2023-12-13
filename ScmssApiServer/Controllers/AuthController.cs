using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CustomControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, UserManager<User> userManager)
            : base(userManager)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<ActionResult<UserDto>> SignIn([FromBody] AuthSignInDto body)
        {
            UserDto? user = await _authService.SignInAsync(body);
            if (user != null)
            {
                return Ok(user);
            }
            return Forbid();
        }

        [HttpPost]
        [Route("SignOut")]
        public new async Task<ActionResult> SignOut()
        {
            await _authService.SignOutAsync();
            return OkMessage("Signed out");
        }
    }
}
