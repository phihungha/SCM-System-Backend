using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ScmssApiServer.DomainExceptions;
using ScmssApiServer.Exceptions;

namespace ScmssApiServer.Services
{
    public class CustomExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is EntityNotFoundException notFoundExc)
            {
                context.Result = new NotFoundObjectResult(notFoundExc.Message);
                context.ExceptionHandled = true;
            }
            else if (context.Exception is InvalidDomainOperationException invalidDomainExc)
            {
                context.Result = new BadRequestObjectResult(invalidDomainExc.Message);
                context.ExceptionHandled = true;
            }
            else if (context.Exception is UnauthorizedException)
            {
                context.Result = new ForbidResult();
                context.ExceptionHandled = true;
            }
            else if (context.Exception is UnauthenticatedException)
            {
                context.Result = new UnauthorizedResult();
                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        { }
    }
}
