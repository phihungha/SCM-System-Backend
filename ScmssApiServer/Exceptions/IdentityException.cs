using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ScmssApiServer.Exceptions
{
    public class IdentityException : ApplicationException, IAddToModelState
    {
        public IEnumerable<IdentityError> Errors { get; }

        public IdentityException(IdentityResult result)
            : base("ASP.NET Identity failed.")
        {
            Errors = result.Errors;
        }

        public IdentityException(IEnumerable<IdentityError> errors)
            : base("ASP.NET Identity failed.")
        {
            Errors = errors;
        }

        public void AddToModelState(ModelStateDictionary modelState)
        {
            foreach (IdentityError error in Errors)
            {
                modelState.TryAddModelError(error.Code, error.Description);
            }
        }
    }
}
