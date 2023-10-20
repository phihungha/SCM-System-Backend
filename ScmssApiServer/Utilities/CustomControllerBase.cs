using Microsoft.AspNetCore.Mvc;

namespace ScmssApiServer.Utilities
{
    public abstract class CustomControllerBase : ControllerBase
    {
        [NonAction]
        public OkObjectResult OkMessage(string message)
        {
            return Ok(new { Title = message });
        }
    }
}
