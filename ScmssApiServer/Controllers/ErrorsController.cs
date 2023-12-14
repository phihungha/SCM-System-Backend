using Microsoft.AspNetCore.Mvc;

namespace ScmssApiServer.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api")]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError() => Problem();
    }
}
