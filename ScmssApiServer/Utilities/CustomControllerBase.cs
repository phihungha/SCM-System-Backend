using Microsoft.AspNetCore.Mvc;

namespace ScmssApiServer.Utilities
{
    public abstract class CustomControllerBase : ControllerBase
    {
        public OkObjectResult OkMessage(string message)
        {
            return Ok(new { Title = message });
        }
    }
}
