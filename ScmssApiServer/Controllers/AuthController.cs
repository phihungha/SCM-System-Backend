using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Utilities;

namespace ScmssApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CustomControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST api/auth/signin
        [HttpPost]
        [Route("SignIn")]
        public async Task<ActionResult> SignIn([FromBody] AuthSignInDto body)
        {
            bool result = await _authService.SignInAsync(body);
            if (result)
            {
                return OkMessage("Signed in");
            }
            return Forbid();
        }

        // POST api/auth/signout
        [HttpPost]
        [Route("SignOut")]
        public new async Task<ActionResult> SignOut()
        {
            await _authService.SignOutAsync();
            return OkMessage("Signed out");
        }
    }
}
