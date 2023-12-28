using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScmssApiServer.DTOs;
using ScmssApiServer.IDomainServices;
using ScmssApiServer.Models;

namespace ScmssApiServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class HomeController : CustomControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService, UserManager<User> userManager)
            : base(userManager)
        {
            _homeService = homeService;
        }

        [HttpGet]
        public async Task<ActionResult<HomeDto>> GetHome()
        {
            HomeDto dto = await _homeService.GetHome();
            return Ok(dto);
        }
    }
}
