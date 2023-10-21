using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ScmssApiServer.Exceptions
{
    public interface IAddToModelState
    {
        void AddToModelState(ModelStateDictionary modelState);
    }
}
